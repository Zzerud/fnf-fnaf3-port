using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FridayNightFunkin.Json;
using Newtonsoft.Json;

namespace FridayNightFunkin
{
    using Song = Json.Song;
    using Note = Json.Note;
    using static FridayNightFunkin.Json.Song;

    public class FNFSong
    {
        public enum DataReadType
        {
            AsLocalFile,
            AsRawJson
        }

        public class FNFSection
        {
            public Note dataNote { get; }

            public List<FNFNote> Notes { get; set; }


            public bool MustHitSection
            {
                get
                {
                    return dataNote.MustHitSection;
                }
                set
                {
                    dataNote.MustHitSection = value;
                }
            }

            public FNFNote ModifyNote(FNFNote toModify, FNFNote newProperties)
            {
                if (toModify == null)
                {
                    throw new Exception("ToModify is null.");
                }

                int index = Notes.IndexOf(toModify);
                dataNote.sectionNotes[index] = newProperties.ConvertToNote();
                Notes[index] = newProperties;
                return newProperties;
            }

            public void AddNote(FNFNote newNote)
            {
                if (newNote == null)
                {
                    throw new Exception("NewNote is null.");
                }

                dataNote.sectionNotes.Add(newNote.ConvertToNote());
                Notes.Add(newNote);
            }

            public void RemoveNote(FNFNote removeNote)
            {
                if (removeNote == null)
                {
                    throw new Exception("RemoveNote is null.");
                }

                List<decimal> list = null;
                foreach (List<decimal> sectionNote in dataNote.sectionNotes)
                {
                    if (!(sectionNote[0] != removeNote.ConvertToNote()[0]) || !(sectionNote[1] != removeNote.ConvertToNote()[1]))
                    {
                        list = sectionNote;
                    }
                }

                if (list == null)
                {
                    throw new Exception("Note not found!");
                }

                dataNote.sectionNotes.Remove(list);
                Notes.Remove(removeNote);
            }

            public FNFSection(Note prvDataNote)
            {
                if (prvDataNote == null)
                {
                    throw new Exception("Song Root is null.");
                }

                dataNote = prvDataNote;
                Notes = new List<FNFNote>();
                foreach (List<decimal> sectionNote in dataNote.sectionNotes)
                {
                    //if (sectionNote.Count <= 3)


                    if (sectionNote.Count >= 4)
                    {

                        if (sectionNote[3] == 404)
                        {
                            Notes.Add(new FNFNote(sectionNote[0], sectionNote[1], sectionNote[2]));

                        }
                        else
                        {
                            Notes.Add(new FNFNote(sectionNote[0], sectionNote[1], sectionNote[2], sectionNote[3]));
                        }
                    }
                    else if (sectionNote.Count == 3)
                        Notes.Add(new FNFNote(sectionNote[0], sectionNote[1], sectionNote[2]));



                }




            }
        }

        public class FNFNote
        {
            

            public decimal Time { get; set; }

            public NoteType Type { get; set; }

            public decimal Length { get; set; }

            public int EventNote { get; set; }

            public List<decimal> ConvertToNote()
            {
                return new List<decimal>
                {
                    Time,
                    (int)Type,
                    Length,
                    EventNote
                };
            }

            public FNFNote(decimal time, decimal noteType, decimal length, decimal eventNote = -1)
            {
                Time = time;
                Type = (NoteType)(int)noteType;
                Length = length;
                EventNote = (int)eventNote;
            }
        }

        public class EventData
        {
            public decimal Time;
            public string[] Details;
        }

        public enum NoteType
        {
            Left,
            Down,
            Up,
            Right,
            RLeft,
            RDown,
            RUp,
            RRight
        }

        private Song.Root dataRoot { get; }

        public List<FNFSection> Sections { get; set; }

        public string SongName
        {
            get
            {
                return dataRoot.song.SongSong;
            }
            set
            {
                dataRoot.song.SongSong = value;
            }
        }

        public long Bpm
        {
            get
            {
                return dataRoot.song.Bpm;
            }
            set
            {
                dataRoot.song.Bpm = value;
            }
        }

        public float Speed
        {
            get
            {
                return dataRoot.song.Speed;
            }
            set
            {
                dataRoot.song.Speed = value;
            }
        }

        public bool NeedVoices
        {
            get
            {
                return dataRoot.song.NeedsVoices;
            }
            set
            {
                dataRoot.song.NeedsVoices = value;
            }
        }

        public string Player1
        {
            get
            {
                return dataRoot.song.Player1;
            }
            set
            {
                dataRoot.song.Player1 = value;
            }
        }

        public string Player2
        {
            get
            {
                return dataRoot.song.Player2;
            }
            set
            {
                dataRoot.song.Player2 = value;
            }
        }

        public string CameraPlayer1
        {
            get
            {
                return dataRoot.song.CameraPlayer1;
            }
            set
            {
                dataRoot.song.CameraPlayer1 = value;
            }
        }
        public string CameraPlayer2
        {
            get
            {
                return dataRoot.song.CameraPlayer2;
            }
            set
            {
                dataRoot.song.CameraPlayer2 = value;
            }
        }

        public EventData[] Events;

        public FNFSong(string data, DataReadType dataReadType = DataReadType.AsLocalFile)
        {
            switch (dataReadType)
            {
                case DataReadType.AsLocalFile:
                    dataRoot = JsonConvert.DeserializeObject<Song.Root>(File.ReadAllText(data));
                    break;
                case DataReadType.AsRawJson:
                    dataRoot = JsonConvert.DeserializeObject<Song.Root>(data);
                    break;
            }

            // Set events to song
            Events = new EventData[dataRoot.song.Events.Count];
            for (int i = 0; i < dataRoot.song.Events.Count; i++)
            {
                EventData eventData = new EventData();
                eventData.Time = Convert.ToDecimal(dataRoot.song.Events[i][0]);
                string initialValue = dataRoot.song.Events[i][1].ToString();
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < initialValue.Length; j++)
                {
                    char c = initialValue[j];
                    if (!(c == '[' || c == ']' || c == '"' || c == '\n' || c == ' '))
                    {
                        sb.Append(c);
                    }
                }
                initialValue = sb.ToString().Trim();
                eventData.Details = initialValue.Split(',');
                for (int j = 0; j < eventData.Details.Length; j++)
                {
                    StringBuilder sbs = new StringBuilder();
                    for (int js = 0; js < eventData.Details[j].Length; js++)
                    {
                        char c = eventData.Details[j][js];
                        if (!(c == '[' || c == ']' || c == '"' || c == '\n' || c == ' '))
                        {
                            sbs.Append(c);
                        }
                    }
                    eventData.Details[j] = sbs.ToString().Trim();
                }
                Events[i] = eventData;
            }

            Sections = new List<FNFSection>();
            Note[] notes = dataRoot.song.Notes.ToArray();
            foreach (Note prvDataNote in notes)
            {
                Sections.Add(new FNFSection(prvDataNote));
            }
        }

        public void SaveSong(string savePath)
        {
            Console.WriteLine("Compiling song...");
            for (int i = 0; i < dataRoot.song.Notes.Count; i++)
            {
                Console.WriteLine("Section " + i);
                FNFSection fNFSection = Sections[i];
                Note note = dataRoot.song.Notes[i];
                note.Bpm = Bpm;
                for (int j = 0; j < fNFSection.Notes.Count; j++)
                {
                    FNFNote fNFNote = fNFSection.Notes[j];
                    Console.WriteLine("Compiling note " + j);
                    note.sectionNotes[j] = fNFNote.ConvertToNote();
                }

                dataRoot.song.Notes[i] = note;
            }

            Console.WriteLine("Compiled! Saving to " + savePath);
            File.WriteAllText(savePath, JsonConvert.SerializeObject(dataRoot));
        }
    }
}