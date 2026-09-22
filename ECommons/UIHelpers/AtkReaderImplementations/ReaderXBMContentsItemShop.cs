using FFXIVClientStructs.FFXIV.Component.GUI;
using System.Collections.Generic;
using System.Linq;

namespace ECommons.UIHelpers.AtkReaderImplementations;

public unsafe class ReaderXBMContentsItemShop(AtkUnitBase* UnitBase, int BeginOffset = 0) : AtkReader(UnitBase, BeginOffset)
{

    public string CoinsString => ReadString(1);
    public uint   Coins       => uint.TryParse(CoinsString.Replace(",", ""), out var value) ? value : 0u;


    public uint             StockCount   => ReadUInt(2) ?? 0u;
    public List<StockEntry> StockEntries => Loop<StockEntry>(3, 5, (int)StockCount);

    public class StockEntry(nint UnitBasePtr, int BeginOffset = 0) : AtkReader(UnitBasePtr, BeginOffset)
    {
        public bool Listed => ReadBool(0) ?? false;

        public uint   Item        => ReadUInt(1) ?? 0u;
        public string PriceString => ReadString(2);
        public uint   Price
        {
            get
            {
                var text = PriceString;
                var ind  = text.IndexOf('(');

                return uint.TryParse(ind >= 0 ? text[..ind].Trim() : text, out var price) ? price : 0;
            }
        }

        public bool Discounted => ReadBool(3) ?? false;
        public bool Bought     => ReadBool(4) ?? false;
    }


    public List<ItemEntry>        ItemEntries      => Loop<ItemEntry>(154, 5, 10);
    public IEnumerable<ItemEntry> ItemEntriesValid => ItemEntries.Where(ie => ie.Id > 0);

    public class ItemEntry(nint UnitBasePtr, int BeginOffset = 0) : AtkReader(UnitBasePtr, BeginOffset)
    {
        public bool   Unk0     => ReadBool(0) ?? false;
        public bool   Sellable => ReadBool(1) ?? false;
        public uint   Unk2     => ReadUInt(2) ?? 0;
        public uint   Id       => ReadUInt(3) ?? 0;
        public string Name     => ReadString(4);
    }


    public List<GearEntry>        OwnedEntries      => Loop<GearEntry>(205, 5, 10);
    public IEnumerable<GearEntry> OwnedEntriesOwned => OwnedEntries.Where(oe => oe.Owned);

    public class GearEntry(nint UnitBasePtr, int BeginOffset = 0) : AtkReader(UnitBasePtr, BeginOffset)
    {
        public bool   Owned => ReadBool(0) ?? false;
        public uint   Unk2  => ReadUInt(2) ?? 0;
        public uint   Id    => ReadUInt(3) ?? 0;
        public string Name  => ReadString(4);
    }
}