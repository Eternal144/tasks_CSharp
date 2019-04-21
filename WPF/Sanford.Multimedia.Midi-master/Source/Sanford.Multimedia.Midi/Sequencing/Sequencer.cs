using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Sanford.Multimedia.Midi
{
    public class Sequencer : IComponent
    {
        private Sequence sequence = null;

        private List<IEnumerator<int>> enumerators = new List<IEnumerator<int>>();

        private MessageDispatcher dispatcher = new MessageDispatcher();

        private ChannelChaser chaser = new ChannelChaser();

        private ChannelStopper stopper = new ChannelStopper();

        private MidiInternalClock clock = new MidiInternalClock();

        private int tracksPlayingCount;

        private readonly object lockObject = new object();

        private bool playing = false;

        private bool disposed = false;

        private ISite site = null;

        #region Events

        public event EventHandler PlayingCompleted;

        public event EventHandler<ChannelMessageEventArgs> ChannelMessagePlayed
        {
            add
            {
                dispatcher.ChannelMessageDispatched += value;
            }
            remove
            {
                dispatcher.ChannelMessageDispatched -= value;
            }
        }

        public event EventHandler<SysExMessageEventArgs> SysExMessagePlayed
        {
            add
            {
                dispatcher.SysExMessageDispatched += value;
            }
            remove
            {
                dispatcher.SysExMessageDispatched -= value;
            }
        }

        public event EventHandler<MetaMessageEventArgs> MetaMessagePlayed
        {
            add
            {
                dispatcher.MetaMessageDispatched += value;
            }
            remove
            {
                dispatcher.MetaMessageDispatched -= value;
            }
        }

        public event EventHandler<ChasedEventArgs> Chased
        {
            add
            {
                chaser.Chased += value;
            }
            remove
            {
                chaser.Chased -= value;
            }
        }

        public event EventHandler<StoppedEventArgs> Stopped
        {
            add
            {
                stopper.Stopped += value;
            }
            remove
            {
                stopper.Stopped -= value;
            }
        }

        #endregion

        public Sequencer()
        {
            dispatcher.MetaMessageDispatched += delegate(object sender, MetaMessageEventArgs e)
            {
                if(e.Message.MetaType == MetaType.EndOfTrack)
                {
                    tracksPlayingCount--;

                    if(tracksPlayingCount == 0)
                    {
                        Stop();

                        OnPlayingCompleted(EventArgs.Empty);
                    }
                }
                else
                {
                    clock.Process(e.Message);
                }
            };

            dispatcher.ChannelMessageDispatched += delegate(object sender, ChannelMessageEventArgs e)
            {
                stopper.Process(e.Message);
            };

            clock.Tick += delegate(object sender, EventArgs e)
            {
                Console.WriteLine("4-B-tick");
                lock(lockObject)
                {
                    Console.WriteLine("4-In-tick");
                    if (!playing)
                    {
                        return;
                    }
                    Console.WriteLine($"4-1-In:{enumerators.Count}");

                    foreach (IEnumerator<int> enumerator in enumerators)
                    {
                        //Console.WriteLine("4-2-In lock in tick");
                        enumerator.MoveNext();
                        //Console.WriteLine("4-3-In lock in tick");
                    }
                }
                Console.WriteLine("4-Aft-tick");

            };
        }

        ~Sequencer()
        {
            Dispose(false);
        }

        //public void Continue()
        //{
        //    if (this.disposed)
        //    {
        //        throw new ObjectDisposedException(base.GetType().Name);
        //    }
        //    if (this.Sequence != null)
        //    {
        //        lock (this.lockObject)
        //        {
        //            this.Stop();
        //            this.enumerators.Clear();
        //            foreach (Track item in this.Sequence)
        //            {
        //                this.enumerators.Add(item.TickIterator(this.Position, this.chaser, this.dispatcher).GetEnumerator());
        //            }
        //            this.tracksPlayingCount = this.Sequence.Count;
        //            this.playing = true;
        //            this.clock.Ppqn = this.sequence.Division;
        //            this.clock.Continue();
        //        }
        //    }
        //}
        public void ContinueReadingTracks()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(base.GetType().Name);
            }
            if (this.Sequence != null)
            {
                Console.WriteLine("5-before lock in ContinueReadingTracks");

                lock (this.lockObject)
                {
                    Console.WriteLine("5-In lock in ContinueReadingTracks");
                    this.enumerators.Clear();
                    foreach (Track item in this.Sequence)
                    {
                        this.enumerators.Add(item.ReaderIterator(this.Position, this.chaser, this.dispatcher).GetEnumerator());
                    }
                    this.tracksPlayingCount = this.Sequence.Count;
                }
                Console.WriteLine("5-After lock in ContinueReadingTracks");

            }
        }

        public void GetTracks()
        {
            this.Stop();
            this.Position = 0;
            this.ContinueReadingTracks();
            foreach (IEnumerator<int> enumerator2 in this.enumerators)
            {
                enumerator2.MoveNext();
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                Console.WriteLine("6-before lock in Dispose");

                lock (lockObject)
                {
                    Console.WriteLine("6-In lock in Dispose");
                    Stop();

                    clock.Dispose();

                    disposed = true;

                    GC.SuppressFinalize(this);
                }
                Console.WriteLine("6-After lock in Dispose");

            }
        }

        public void Start()
        {
            #region Require

            if(disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }

            #endregion

            Console.WriteLine($"1-Before lock in Start");
            lock (lockObject)
            {
                Console.WriteLine($"1-in lock in Start");
                Stop();

                Position = 0;

                Continue();
            }
            Console.WriteLine($"1-After lock in Start");
        }

        public void Continue()
        {
            #region Require

            if(disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }

            #endregion

            #region Guard

            if(Sequence == null)
            {
                return;
            }

            #endregion

            Console.WriteLine($"2-before lock in Continue");
            lock (lockObject)
            {
                Console.WriteLine($"2-in lock in Continue");
                Stop();

                enumerators.Clear();

                foreach(Track t in Sequence)
                {
                    enumerators.Add(t.TickIterator(Position, chaser, dispatcher).GetEnumerator());
                }

                tracksPlayingCount = Sequence.Count;

                playing = true;
                clock.Ppqn = sequence.Division;
                clock.Continue();
            }
            Console.WriteLine($"2-After lock in Continue");
        }

        public void Stop()
        {
            #region Require

            if(disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }

            #endregion

            Console.WriteLine($"3-Before lock in Stop");
            lock (lockObject)
            {
                Console.WriteLine($"3-in lock in Stop");
                #region Guard

                if (!playing)
                {
                    return;
                }

                #endregion

                playing = false;
                clock.Stop();
                stopper.AllSoundOff();
            }
            Console.WriteLine($"3-After lock in Stop");

        }

        protected virtual void OnPlayingCompleted(EventArgs e)
        {
            EventHandler handler = PlayingCompleted;

            if(handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnDisposed(EventArgs e)
        {
            EventHandler handler = Disposed;

            if(handler != null)
            {
                handler(this, e);
            }
        }

        public int Position
        {
            get
            {
                #region Require

                if(disposed)
                {
                    throw new ObjectDisposedException(this.GetType().Name);
                }

                #endregion

                return clock.Ticks;
            }
            set
            {
                #region Require

                if(disposed)
                {
                    throw new ObjectDisposedException(this.GetType().Name);
                }
                else if(value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                #endregion

                bool wasPlaying;

                Console.WriteLine("7-before lock in Positon");

                lock (lockObject)
                {
                    Console.WriteLine("7-in lock in Positon");
                    wasPlaying = playing;

                    Stop();

                    clock.SetTicks(value);
                }
                Console.WriteLine("7-after lock in Positon");

                Console.WriteLine("7A-Before lock in Positon");
                lock (lockObject)
                {
                    Console.WriteLine("7A-In lock in Positon");
                    if (wasPlaying)
                    {
                        Continue();
                    }
                }
                Console.WriteLine("7A-After lock in Positon");
            }
        }

        public Sequence Sequence
        {
            get
            {
                return sequence;
            }
            set
            {
                #region Require

                if(value == null)
                {
                    throw new ArgumentNullException();
                }
                else if(value.SequenceType == SequenceType.Smpte)
                {
                    throw new NotSupportedException();
                }

                #endregion
                Console.WriteLine("8-Before lock in Sequence");

                lock (lockObject)
                {
                    Console.WriteLine("8-In lock in Sequence");
                    Stop();
                    sequence = value;
                }
                Console.WriteLine("8-After lock in Sequence");
            }
        }

        #region IComponent Members

        public event EventHandler Disposed;

        public ISite Site
        {
            get
            {
                return site;
            }
            set
            {
                site = value;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            #region Guard

            if(disposed)
            {
                return;
            }

            #endregion

            Dispose(true);
        }

        #endregion
    }
}
