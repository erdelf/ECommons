﻿using Dalamud.Game.Text.SeStringHandling;
using ECommons.Automation;
using ECommons.UIHelpers.AtkReaderImplementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommons.UIHelpers.AddonMasterImplementations;
public unsafe partial class AddonMaster
{
    public class DawnStory : AddonMasterBase
    {
        public DawnStory(nint addon) : base(addon)
        {
        }

        public DawnStory(void* addon) : base(addon)
        {
        }

        public override string AddonDescription { get; } = "Duty Support window";
        public ReaderDawnStory Reader => new(this.Base);
        public IEnumerable<Entry> Entries
        {
            get
            {
                for(int i = 0; i < Reader.EntryCount; i++)
                {
                    if(Reader.Entries[i].Callback != 0)
                    {
                        yield return new(this, i);
                    }
                }
            }
        }

        public class Entry(DawnStory m, int index)
        {
            public ReaderDawnStory.Entry ReaderEntry => m.Reader.Entries[index];
            public ReaderDawnStory.EntryName ReaderEntryName => m.Reader.EntryNames[index];
            public SeString Name => ReaderEntryName.Name;
            public uint Status => ReaderEntry.Status;
            public int Index => index;
            public void Select()
            {
                var c = m.Reader.Entries[index].Callback;
                if(c != 0)
                {
                    Callback.Fire(m.Base, true, 12, c);
                }
            }
        }
    }
}