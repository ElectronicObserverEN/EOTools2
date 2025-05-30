using System.ComponentModel.DataAnnotations;

namespace EOToolsWeb.Shared.EquipmentUpgrades;

public enum UpgradeLevel
{
    /// <summary>1</summary>
    [Display(Name = "0->1")]
    One = 1,

    /// <summary>2</summary>
    [Display(Name = "1->2")]
    Two = 2,

    /// <summary>3</summary>
    [Display(Name = "2->3")]
    Three = 3,

    /// <summary>4</summary>
    [Display(Name = "3->4")]
    Four = 4,

    /// <summary>5</summary>
    [Display(Name = "4->5")]
    Five = 5,

    /// <summary>6</summary>
    [Display(Name = "5->6")]
    Six = 6,

    /// <summary>7</summary>
    [Display(Name = "6->7")]
    Seven = 7,

    /// <summary>8</summary>
    [Display(Name = "7->8")]
    Eight = 8,

    /// <summary>9</summary>
    [Display(Name = "8->9")]
    Nine = 9,

    /// <summary>Max</summary>
    [Display(Name = "9->10")]
    Max = 10,

    /// <summary>Equipment conversion</summary>
    [Display(Name = "Conversion")]
    Conversion = 255
}
