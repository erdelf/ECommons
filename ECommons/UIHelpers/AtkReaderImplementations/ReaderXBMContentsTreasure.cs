using Dalamud.Game.Text.SeStringHandling;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System.Collections.Generic;
using System.Linq;

namespace ECommons.UIHelpers.AtkReaderImplementations;

public unsafe class ReaderXBMContentsTreasure(AtkUnitBase* UnitBase, int BeginOffset = 0) : AtkReader(UnitBase, BeginOffset)
{

    public string CoinsString => ReadString(2);
    public uint Coins => uint.TryParse(CoinsString.Replace(",", ""), out var value) ? value : 0u;

    private const int TreasureOffset = 3;
    private const int TreasureEntrySize = 5;

    public List<TreasureChoice> TreasureChoices => Loop<TreasureChoice>(TreasureOffset, TreasureEntrySize, (int)4);

    public class TreasureChoice(nint UnitBasePtr, int BeginOffset = 0) : AtkReader(UnitBasePtr, BeginOffset)
    {
        public readonly int treasureIndex = (BeginOffset - TreasureOffset) / TreasureEntrySize;

        public bool Unk0 => ReadBool(0) ?? false;
        public uint Item => ReadUInt(3) ?? 0u;
        public bool Bought => ReadBool(126 + treasureIndex - this.AtkReaderParams.BeginOffset) ?? false;
    }


    public List<ItemEntry> ItemEntries => Loop<ItemEntry>(24, 5, 10);
    public IEnumerable<ItemEntry> ItemEntriesValid => ItemEntries.Where(ie => ie.Id > 0);

    public class ItemEntry(nint UnitBasePtr, int BeginOffset = 0) : AtkReader(UnitBasePtr, BeginOffset)
    {
        public bool Unk0 => ReadBool(0) ?? false;
        public bool Sellable => ReadBool(1) ?? false;
        public uint IconId => ReadUInt(2) ?? 0;
        public uint Id => ReadUInt(3) ?? 0;
        public string Name => ReadString(4);
    }


    public List<GearEntry> OwnedEntries => Loop<GearEntry>(75, 5, 10);
    public IEnumerable<GearEntry> OwnedEntriesOwned => OwnedEntries.Where(oe => oe.Owned);

    public class GearEntry(nint UnitBasePtr, int BeginOffset = 0) : AtkReader(UnitBasePtr, BeginOffset)
    {
        public bool Owned => ReadBool(0) ?? false;
        public uint Unk2 => ReadUInt(2) ?? 0;
        public uint Id => ReadUInt(3) ?? 0;
        public string Name => ReadString(4);
    }
}