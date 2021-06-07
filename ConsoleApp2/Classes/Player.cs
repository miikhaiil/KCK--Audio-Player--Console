using CSCore;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using System;
using CSCore.Streams;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CSCore.Streams.Effects;
using CSCore.Codecs.WAV;
using CSCore.MediaFoundation;
using CSCore.Codecs.MP3;

namespace ConsoleApp2
{
    class Player
    {   
        private static Player instance;
        private Player()
        {
            playlist = new List<string>();
            songsTime = new List<TimeSpan>();
        }
        private List<string> playlist;
        private List<TimeSpan> songsTime;
        private static Random rng = new Random();
        private ISoundOut _soundOut;
        private TimeSpan span;
        private static IWaveSource _soundSource;
        private float volume=0.8f;
        public Boolean flag=false;
        public int currentSong = 0;
        public double gain = 1d;
        public Boolean isPlaying = false;
        public Boolean isPaused = false;
        public Boolean isLooped = false;
        public Boolean playerWorking = false;
        private Equalizer eq;
        private double[] gains = new double[10];
        public void gainUp(int i)
        {
            if (gains[i] < 20)
            {
                gains[i] += gain;
                if (eq != null)
                {
                    EqualizerFilter filter = eq.SampleFilters[i];
                    filter.AverageGainDB = filter.AverageGainDB - gain;
                }
            }
        }

        public void gainDown(int i)
        {
            if (gains[i] > -20)
            {
                gains[i] -= gain;
                if(eq != null)
                {
                    EqualizerFilter filter = eq.SampleFilters[i];
                    filter.AverageGainDB = filter.AverageGainDB - gain;
                }
            }
        }

        public TimeSpan getSongTime(int i)
        {
            return songsTime[i];
        }

        public void resetEqualizer()
        {
            int i = 0;
            foreach (EqualizerFilter f in eq.SampleFilters)
            {
                f.AverageGainDB = 0;
                gains[i] = 0;
                i++;
            }
        }

        public double getGain(int index)
        {
            return gains[index];
 
        }

        public static Player getInstance()
        {
            if (instance == null)
            {
                instance = new Player();
            }
            return instance;
        }

        public bool checkTime()
        {
            if (_soundOut != null && _soundOut.WaveSource != null)
            {
                if (_soundOut.WaveSource.GetPosition() >= _soundOut.WaveSource.GetLength())
                {
                    if(isLooped)
                    {
                        _soundOut.WaveSource.SetPosition(new TimeSpan(0));
                        _soundOut.Play();
                        return false;
                    }
                    else
                        playNext();
                    return true;
                }
            }
            return false;
        }

        #region INICJALIZACJA
        public void initialize(string file)
        {
            playerWorking = true;
            _soundSource = CodecFactory.Instance.GetCodec(file);
            eq = Equalizer.Create10BandEqualizer(_soundSource.ToSampleSource());
            int i = 0;
            foreach (EqualizerFilter f in eq.SampleFilters)
            {
                f.AverageGainDB = gains[i];
                i++;
            }
            _soundOut = GetSoundOut();
            _soundOut.Initialize(eq.ToWaveSource());
            _soundOut.Volume = volume;
        }

        private static ISoundOut GetSoundOut()
        {
            if (WasapiOut.IsSupportedOnCurrentPlatform)
            {
                return new WasapiOut
                {
                    Device = GetDevice()
                };
            }

            return new DirectSoundOut();
        }

        public static MMDevice GetDevice()
        {
            using (var mmdeviceEnumerator = new MMDeviceEnumerator())
            {
                using (var mmdeviceCollection = mmdeviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
                {
                    return mmdeviceCollection.LastOrDefault();
                }
            }
        }
        #endregion

        #region ODTWARZAANIE
        private void playPause()
        {
            if (_soundOut != null)
            {
                if (isPaused == false)
                {
                    _soundOut.Pause();
                    isPaused = true;
                }
                else
                {
                    _soundOut.Resume();
                    isPaused = false;
                }
            }
        }

        public void choosePlay()
        {
            if(currentSong>=0 && currentSong<playlist.Count())
            {
                Stop();
                initialize(playlist[currentSong]);
                isPlaying = true;
                _soundOut.Play();
                flag = true;
            }  
        }
        public void Play()
        {
            if (isPlaying == false)
            {
                initialize(playlist[currentSong]);
                if (_soundOut != null)
                {
                    isPlaying = true;
                    _soundOut.Volume = volume;
                    _soundOut.Play();
                    flag = true;
                }
                
            }
            else
            {
                playPause();
            }
        }

        public void Stop()
        {
            if (_soundOut != null)
            {
                flag = false;
                _soundOut.Dispose();
                _soundSource.Dispose();
                isPlaying = false;
                isPaused = false;
            }
        }
        #endregion

        #region SKIPOWANIE
        public void playNext()
        {
            currentSong++;
            currentSong = currentSong % playlist.Count();
            if(isPlaying==true)
            {
                choosePlay();
            }
            
        }

        public void playPrevious()
        {
            if(currentSong==0)
            {
                currentSong = playlist.Count();
            }
            currentSong--;
            if (isPlaying == true)
            {
                choosePlay();
            }

        }
        #endregion

        #region PRZEWIJANIE
        public void FastForward()
        {
            if(_soundOut != null && _soundOut.WaveSource!=null)
            {
 
                span = _soundOut.WaveSource.GetPosition()+TimeSpan.FromMilliseconds(500);
                
                _soundOut.WaveSource.SetPosition(span);
                
            }
       }

        public void Rewind()
        {
            if (_soundOut != null && _soundOut.WaveSource != null)
            {
                
                if (TimeSpan.Compare(_soundOut.WaveSource.GetPosition(),TimeSpan.FromMilliseconds(500))>0)
                {
                    
                    span = _soundOut.WaveSource.GetPosition() - TimeSpan.FromMilliseconds(500);
                    _soundOut.WaveSource.SetPosition(span);
                   
                }   
            }
        }
        #endregion

        #region ZMIANA GŁOŚNOŚCI
        public void volumeUp()
        {
            if(_soundOut != null)
            {
                if(volume<=0.98f)
                {
                    volume= volume + 0.02f;
                    _soundOut.Volume = volume;
                }
            }
        }

        public void volumeDown()
        {
            if (_soundOut != null)
            {
                if(volume>=0.02f)
                {
                    volume= volume - 0.02f;
                    _soundOut.Volume = volume;
                }
                   
            }
        }
        #endregion

        #region PLAYLIST
        public List<string> getPlaylist()
        {
            return playlist;
        }
        public int getPlaylistSize()
        {
            return playlist.Count();
        }

        public void savePlaylist(string name)
        {
            File.WriteAllLines(name + ".plr", playlist);
        }

        public void addToPlaylist(string path)
        {
            playlist.Add(path);
            MediaFoundationDecoder decoder = new Mp3MediafoundationDecoder(path);
            songsTime.Add(decoder.GetLength());
        }
        public void loadPlaylist(string path)
        {
            if (Path.GetExtension(path) == ".plr")
            {
                playlist.Clear();
                songsTime.Clear();
                var files = File.ReadAllLines(path);
                foreach (var s in files)
                {
                    if(File.Exists(s))
                    {
                        playlist.Add(s);
                        MediaFoundationDecoder decoder = new Mp3MediafoundationDecoder(s);
                        songsTime.Add(decoder.GetLength());
                    }
                }
            }
        }
        public void shufflePlaylist()
        {
            if(isPlaying)
            {
                Stop();
            }
            currentSong = 0;
            int listLength = playlist.Count;
            while (listLength > 1)
            {
                listLength--;
                int k = rng.Next(listLength + 1);
                string path = playlist[k];
                playlist[k] = playlist[listLength];
                playlist[listLength] = path;
            }
        }
        #endregion
    }
}
