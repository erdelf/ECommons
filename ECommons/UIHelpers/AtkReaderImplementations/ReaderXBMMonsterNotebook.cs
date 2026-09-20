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

    public SeString SelectedNumberString => ReadSeString(229);

    public uint SelectedNumber
    {
        get
        {
            var text = SelectedNumberString.GetText();
            var ind  = text.IndexOf('.');

            return uint.TryParse(text[(ind + 1)..].Trim(), out var selectedNumber) ? selectedNumber : 0;
        }
    }

    public bool Selected => ReadBool(255) ?? false;

    public SeString SelectedName       => ReadSeString(230);
    public SeString Classification     => ReadSeString(239);


    public SeString SelectedRankString => ReadSeString(258);
    public int      SelectedRank       => int.TryParse(SelectedRankString.GetText().Trim(), out var rank) ? rank : 0;
    public uint     SelectedXP         => ReadUInt(261) ?? 0;
    public SeString SelectedHP => ReadSeString(259);
    public int SelectedMaxHP
    {
        get
        {
            var text = SelectedHP.GetText();
            var ind  = text.IndexOf('/');

            return int.TryParse(text[(ind + 1)..].Trim(), out var hp) ? hp : 0;
        }
    }
    public SeString SelectedStrengthString => ReadSeString(265);
    public int      SelectedStrength       => int.TryParse(SelectedStrengthString.GetText().Trim(), out var i) ? i : 0;

    public SeString SelectedPhysResistanceString => ReadSeString(267);
    public int SelectedPhysResistance => int.TryParse(SelectedPhysResistanceString.GetText().Trim(), out var i) ? i : 0;

    public SeString SelectedConstitutionString => ReadSeString(269);
    public int SelectedConstitution => int.TryParse(SelectedConstitutionString.GetText().Trim(), out var i) ? i : 0;

    public SeString SelectedIntelligenceString => ReadSeString(271);
    public int SelectedIntelligence => int.TryParse(SelectedIntelligenceString.GetText().Trim(), out var i) ? i : 0;

    public SeString SelectedMagicResistanceString => ReadSeString(273);
    public int SelectedMagicResistance => int.TryParse(SelectedMagicResistanceString.GetText().Trim(), out var i) ? i : 0;
}