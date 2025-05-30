using System.ComponentModel.DataAnnotations;

namespace EOToolsWeb.Shared.EquipmentUpgrades;

public enum UpgradeLevel
{
    /// <summary>1</summary>
    [Display(Name = "0->1")]
    ZeroToOne = 1,

    /// <summary>2</summary>
    [Display(Name = "1->2")]
    OneToTwo = 2,

    /// <summary>3</summary>
    [Display(Name = "2->3")]
    TwoToThree = 3,

    /// <summary>4</summary>
    [Display(Name = "3->4")]
    ThreeToFour = 4,

    /// <summary>5</summary>
    [Display(Name = "4->5")]
    FourToFive = 5,

    /// <summary>6</summary>
    [Display(Name = "5->6")]
    FiveToSix = 6,

    /// <summary>7</summary>
    [Display(Name = "6->7")]
    SixToSeven = 7,

    /// <summary>8</summary>
    [Display(Name = "7->8")]
    SevenToEight = 8,

    /// <summary>9</summary>
    [Display(Name = "8->9")]
    EightToNine = 9,

    /// <summary>Max</summary>
    [Display(Name = "9->10")]
    NineToMax = 10,

    /// <summary>Equipment conversion</summary>
    [Display(Name = "Conversion")]
    Conversion = 255
}
