using Dalamud.Game.Text.SeStringHandling;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System.Collections.Generic;

namespace ECommons.UIHelpers.AtkReaderImplementations;

public unsafe class ReaderXBMMonsterNotebook(AtkUnitBase* UnitBase, int BeginOffset = 0) : AtkReader(UnitBase, BeginOffset)
{
    public const int EntryCountPerPage = 25;

    public uint               PageCount          => ReadUInt(9)  ?? 0;
    public uint               CurrentPage        => ReadUInt(10) ?? 0;
    public List<MonsterEntry> CurrentPageEntries => Loop<MonsterEntry>(24, 8, EntryCountPerPage);

    public class MonsterEntry(nint UnitBasePtr, int BeginOffset = 0) : AtkReader(UnitBasePtr, BeginOffset)
    {
        public uint     Number       => ReadUInt(0) ?? 0u;
        public bool     Unk1         => ReadBool(1) ?? false;
        public bool     Caught       => ReadBool(2) ?? false;
        public bool     Unk3         => ReadBool(3) ?? false;
        public uint     Unk4         => ReadUInt(4) ?? 0u;
        public SeString NumberString => ReadSeString(5);
        public bool     Unk6         => ReadBool(6) ?? false;
        public uint     Unk7         => ReadUInt(7) ?? 0;
    }
}