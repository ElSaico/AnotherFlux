﻿// ReSharper disable InconsistentNaming

using System.Collections.Generic;
using PSVRender;

namespace FluxShared
{
	public class GlobalShared : SNES
	{
		public delegate void PostStatusDel(string sStatus);
		public delegate string GetStrFromGroupDel(StrRecType nGroup, ushort nIndex);
		public static string[] sLocation = new string[512];
		public static string[] sDefLocation = {
			"{000} Load Screen",
			"{001} Crono's Kitchen",
			"{002} Crono's Room",
			"{003} Lucca's Kitchen",
			"{004} Lucca's Workshop",
			"{005} Millennial Fair",
			"{006} Gato's Exhibit",
			"{007} Prehistoric Exhibit",
			"{008} Telepod Exhibit",
			"{009} Lara's Room",
			"{00A} Lucca's Room",
			"{00B} Ending Selector",
			"{00C} Truce Inn (Present)",
			"{00D} Truce Mayor's Manor 1F",
			"{00E} Truce Mayor's Manor 2F",
			"{00F} Truce Single Woman Residence",
			"{010} Truce Happy Screaming Couple Residence",
			"{011} Truce Market (Present)",
			"{012} Truce Ticket Office",
			"{013} Guardia Forest (Present)",
			"{014} Guardia Forest Dead End",
			"{015} Guardia Throneroom (Present)",
			"{016} King's Chamber (Present)",
			"{017} Queen's Chamber (Present)",
			"{018} Guardia Kitchen (Present)",
			"{019} Guardia Barracks (Present)",
			"{01A} Guardia Basement",
			"{01B} Courtroom",
			"{01C} Prison Catwalks",
			"{01D} Prison Supervisor's Office",
			"{01E} Prison Torture Storage Room",
			"{01F} Medina Square",
			"{020} Zenan Bridge (Present)",
			"{021} Medina Elder's House 1F",
			"{022} Medina Elder's House 2F",
			"{023} Medina Inn",
			"{024} Medina Portal",
			"{025} Ending: Legendary Hero",
			"{026} Guardia Throneroom (Ending: Legendary Hero)",
			"{027} Medina Market",
			"{028} Melchior's Kitchen",
			"{029} Melchior's Workshop",
			"{02A} Forest Ruins",
			"{02B} Cursed Woods (Ending: The Unknown Past)",
			"{02C} Leene Square (Future)",
			"{02D} Denadoro Mountain Vista (Ending: Legendary Hero)",
			"{02E} Castle Magus Throne of Defense (Ending: Legendary Hero)",
			"{02F} Heckran Cave Passageways",
			"{030} Heckran Cave Entrance",
			"{031} Heckran Cave Underground River",
			"{032} Porre Mayor's Manor 1F (Present)",
			"{033} Porre Mayor's Manor 2F (Present)",
			"{034} Porre Residence (Present)",
			"{035} Snail Stop",
			"{036} Porre Market (Present)",
			"{037} Porre Inn (Present)",
			"{038} Porre Ticket Office",
			"{039} Fiona's Shrine",
			"{03A} Choras Mayor's Manor 1F",
			"{03B} Choras Mayor's Manor 2F",
			"{03C} Castle Magus Hall of Aggression (Ending: Legendary Hero)",
			"{03D} Choras Carpenter's Residence (Present)",
			"{03E} Choras Inn (Present)",
			"{03F} West Cape",
			"{040} Sun Keep (Present)",
			"{041} Northern Ruins Entrance",
			"{042} Northern Ruins Basement Corridor",
			"{043} Northern Ruins Landing",
			"{044} Northern Ruins Antechamber",
			"{045} Northern Ruins Vestibule",
			"{046} Northern Ruins Back Room",
			"{047} Prison Cells",
			"{048} Prison Stairwells",
			"{049} Northern Ruins Hero's Grave (Present)",
			"{04A} Unknown",
			"{04B} Prison Exterior",
			"{04C} Unknown (Lavos Spawn)",
			"{04D} Unknown (Lavos Spawn)",
			"{04E} Unknown (Lavos Spawn)",
			"{04F} Guardia Throneroom (Ending: Moonlight Parade)",
			"{050} Millennial Fair (Ending: Moonlight Parade)",
			"{051} Leene Square (Ending: Moonlight Parade)",
			"{052} Ending Selector",
			"{053} Fiona's Forest Recriminations",
			"{054} End of Time (Ending: Moonlight Parade)",
			"{055} Telepod Exhibit (Ending: Moonlight Parade)",
			"{056} Death Peak Summit (Ending: Moonlight Parade)",
			"{057} Manoria Sanctuary (Ending: The Successor of Guardia)",
			"{058} The End",
			"{059} Zenan Bridge (Ending: Moonlight Parade)",
			"{05A} Prison Catwalks (Ending: Moonlight Parade)",
			"{05B} Ending: People of the Times (Part 1)",
			"{05C} Ending: People of the Times (Part 2)",
			"{05D} Ending: People of the Times (Part 3)",
			"{05E} Ending: People of the Times (Part 4)",
			"{05F} Ending: People of the Times (Part 5)",
			"{060} Black Omen 98F Astral Progeny",
			"{061} Black Omen Lower Teleporters",
			"{062} Black Omen Upper Teleporters",
			"{063} Black Omen Platform",
			"{064} Black Omen Platform Shaft (Downward)",
			"{065} Black Omen Platform Shaft (Upward)",
			"{066} Black Omen Celestial Gate (no map)",
			"{067} Black Omen Celestial Gate (no map)",
			"{068} Black Omen Celestial Gate (no map)",
			"{069} Black Omen Celestial Gate (no map)",
			"{06A} Black Omen Celestial Gate (no map)",
			"{06B} Black Omen Celestial Gate",
			"{06C} Lucca Explains Paradoxes",
			"{06D} Ancient Tyrano Lair",
			"{06E} Ancient Tyrano Lair Traps",
			"{06F} Ancient Tyrano Lair Nizbel's Room",
			"{070} Truce Canyon",
			"{071} Truce Canyon Portal",
			"{072} Truce Couple's Residence (Middle Ages)",
			"{073} Truce Smithy's Residence",
			"{074} Truce Inn 1F (Middle Ages)",
			"{075} Truce Inn 2F (Middle Ages)",
			"{076} Truce Market (Middle Ages)",
			"{077} Guardia Forest (Middle Ages)",
			"{078} Guardia Throneroom (Middle Ages)",
			"{079} Guardia King's Chamber (Middle Ages)",
			"{07A} Guardia Queen's Chamber (Middle Ages)",
			"{07B} Guardia Kitchen (Middle Ages)",
			"{07C} Guardia Barracks (Middle Ages)",
			"{07D} Castle Magus Doppleganger Corridor",
			"{07E} Geno Dome Main Conveyor",
			"{07F} Geno Dome Elevator",
			"{080} Geno Dome Long Corridor",
			"{081} Manoria Sanctuary",
			"{082} Manoria Main Hall",
			"{083} Manoria Headquarters",
			"{084} Manoria Royal Guard Hall",
			"{085} Zenan Bridge (Wrecked)",
			"{086} Zenan Bridge (Middle Ages, no map)",
			"{087} Zenan Bridge (Middle Ages)",
			"{088} Sandorino Pervert Residence",
			"{089} Sandorino Elder's House",
			"{08A} Sandorino Inn",
			"{08B} Sandorino Market",
			"{08C} Cursed Woods",
			"{08D} Frog's Burrow",
			"{08E} Denadoro South Face",
			"{08F} Denadoro Cave of the Masamune Exterior",
			"{090} Denadoro North Face",
			"{091} Denadoro Entrance",
			"{092} Denadoro Lower East Face",
			"{093} Denadoro Upper East Face",
			"{094} Denadoro Mountain Vista",
			"{095} Denadoro West Face",
			"{096} Denadoro Gauntlet",
			"{097} Denadoro Cave of the Masamune",
			"{098} Tata's House 1F",
			"{099} Tata's House 2F",
			"{09A} Porre Elder's House (Middle Ages)",
			"{09B} Porre Cafe (Middle Ages)",
			"{09C} Porre Inn (Middle Ages)",
			"{09D} Porre Market (Middle Ages)",
			"{09E} Fiona's Villa",
			"{09F} Sunken Desert Entrance",
			"{0A0} Sunken Desert Parasytes",
			"{0A1} Sunken Desert Devourer",
			"{0A2} Ozzie's Fort Entrance (no map)",
			"{0A3} Magic Cave Exterior",
			"{0A4} Magic Cave Interior",
			"{0A5} Castle Magus Exterior",
			"{0A6} Castle Magus Entrance",
			"{0A7} Castle Magus Chamber of Guillotines",
			"{0A8} Castle Magus Chamber of Pits",
			"{0A9} Castle Magus Throne of Strength",
			"{0AA} Castle Magus Hall of Aggression",
			"{0AB} Castle Magus Hall of Deceit",
			"{0AC} Castle Magus Inner Sanctum",
			"{0AD} Castle Magus Throne of Magic",
			"{0AE} Castle Magus Throne of Defense",
			"{0AF} Castle Magus Hall of Apprehension",
			"{0B0} Castle Magus Lower Battlements",
			"{0B1} Ozzie's Fort Entrance",
			"{0B2} Ozzie's Fort Hall of Disregard",
			"{0B3} Ozzie's Fort Chamber of Kitchen Knives",
			"{0B4} Ozzie's Fort Last Stand",
			"{0B5} Ozzie's Fort Throne of Incompetence",
			"{0B6} Ozzie's Fort Throne of Impertinence (wrong map)",
			"{0B7} Ozzie's Fort Throne of Impertinence",
			"{0B8} Ozzie's Fort Throne of Ineptitude",
			"{0B9} Choras Old Couple Residence (Middle Ages)",
			"{0BA} Choras Carpenter's Residence 1F (Middle Ages)",
			"{0BB} Choras Carpenter's Residence 2F (Middle Ages)",
			"{0BC} Choras Cafe",
			"{0BD} Choras Inn (Middle Ages)",
			"{0BE} Choras Market (Middle Ages)",
			"{0BF} Sun Keep (Middle Ages)",
			"{0C0} (empty map)",
			"{0C1} (empty map)",
			"{0C2} (empty map)",
			"{0C3} Giant's Claw Entrance",
			"{0C4} Giant's Claw Caverns",
			"{0C5} Giant's Claw Last Tyranno",
			"{0C6} Manoria Command",
			"{0C7} Manoria Confinement",
			"{0C8} Manoria Shrine Antechamber",
			"{0C9} Manoria Storage",
			"{0CA} Manoria Kitchen",
			"{0CB} Manoria Shrine",
			"{0CC} Guardia Forest Frog King Battle",
			"{0CD} Denadoro Cyrus's Last Battle",
			"{0CE} Guardia Throneroom Cyrus's Final Mission",
			"{0CF} Schala's Room (no map)",
			"{0D0} Bangor Dome",
			"{0D1} Bangor Dome Sealed Room",
			"{0D2} Trann Dome",
			"{0D3} Trann Dome Sealed Room",
			"{0D4} Lab 16 West",
			"{0D5} Lab 16 East",
			"{0D6} Arris Dome",
			"{0D7} Arris Dome Infestation",
			"{0D8} Arris Dome Auxiliary Console",
			"{0D9} Arris Dome Lower Commons",
			"{0DA} Arris Dome Command Central",
			"{0DB} Arris Dome Guardian Chamber",
			"{0DC} Arris Dome Sealed Room",
			"{0DD} Arris Dome Rafters",
			"{0DE} Reptite Lair 2F",
			"{0DF} Lab 32 West Entrance",
			"{0E0} Lab 32",
			"{0E1} Lab 32 East Entrance",
			"{0E2} Proto Dome",
			"{0E3} Proto Dome Portal",
			"{0E4} Factory Ruins Entrance",
			"{0E5} Factory Ruins Auxiliary Console",
			"{0E6} Factory Ruins Security Center",
			"{0E7} Factory Ruins Crane Room",
			"{0E8} Factory Ruins Infestation",
			"{0E9} Factory Ruins Crane Control Room",
			"{0EA} Factory Ruins Information Archive",
			"{0EB} Factory Ruins Power Core",
			"{0EC} Sewer Access B1",
			"{0ED} Sewer Access B2",
			"{0EE} Prehistoric Hut (Ending: The Dream Project) (wrong map)",
			"{0EF} Ending: A Slide Show?",
			"{0F0} Ending: Goodnight",
			"{0F1} Keeper's Dome",
			"{0F2} Keeper's Dome Corridor",
			"{0F3} Keeper's Dome Hanger",
			"{0F4} Death Peak Entrance",
			"{0F5} Death Peak South Face",
			"{0F6} Death Peak Southeast Face",
			"{0F7} Death Peak Northeast Face",
			"{0F8} Geno Dome Entrance",
			"{0F9} Geno Dome Conveyor Entrance",
			"{0FA} Geno Dome Conveyor Exit",
			"{0FB} Sun Palace",
			"{0FC} Millennial Fair (Ending: Dino Age)",
			"{0FD} Sun Keep (Last Village)",
			"{0FE} Skill Tutorial",
			"{0FF} Sun Keep (Future)",
			"{100} Geno Dome Labs",
			"{101} Geno Dome Storage",
			"{102} Geno Dome Robot Hub",
			"{103} Factory Ruins Data Core",
			"{104} Death Peak Northwest Face",
			"{105} Prehistoric Canyon",
			"{106} Death Peak Upper North Face",
			"{107} Death Peak Lower North Face",
			"{108} Death Peak Cave",
			"{109} Death Peak Summit",
			"{10A} Prehistoric Hut (Ending: The Dream Project) (wrong map)",
			"{10B} Geno Dome Robot Elevator Access",
			"{10C} Geno Dome Mainframe",
			"{10D} Geno Dome Waste Disposal",
			"{10E} Leene's Square (Ending: Dino Age)",
			"{10F} Special Purpose Area",
			"{110} Mystic Mtn Portal",
			"{111} Mystic Mtn Base",
			"{112} Mystic Mtn Gulch",
			"{113} Chief's Hut",
			"{114} Ioka Southwestern Hut",
			"{115} Ioka Trading Post",
			"{116} Ioka Sweet Water Hut",
			"{117} Ioka Meeting Site",
			"{118} Ioka Meeting Site (Party)",
			"{119} Forest Maze Entrance",
			"{11A} Forest Maze",
			"{11B} Reptite Lair Entrance",
			"{11C} Reptite Lair 1F",
			"{11D} Reptite Lair Weevil Burrows B1",
			"{11E} Reptite Lair Weevil Burrows B2",
			"{11F} Reptite Lair Commons",
			"{120} Reptite Lair Tunnel",
			"{121} Reptite Lair Azala's Room",
			"{122} Reptite Lair Access Shaft",
			"{123} Hunting Range",
			"{124} Laruba Ruins",
			"{125} Dactyl Nest, Lower",
			"{126} Dactyl Nest, Upper",
			"{127} Dactyl Nest Summit",
			"{128} Giant's Claw Lair Entrance",
			"{129} Giant's Claw Lair Throneroom",
			"{12A} Tyrano Lair Exterior",
			"{12B} Tyrano Lair Entrance",
			"{12C} Tyrano Lair Throneroom",
			"{12D} Tyrano Lair Keep",
			"{12E} Tyrano Lair Antechambers",
			"{12F} Tyrano Lair Storage",
			"{130} Tyrano Lair Nizbel's Room",
			"{131} Tyrano Lair Room of Vertigo",
			"{132} Debug Room?",
			"{133} Lair Ruins Portal",
			"{134} Black Omen 1F Entrance",
			"{135} Black Omen 1F Walkway",
			"{136} Black Omen 1F Defense Corridor",
			"{137} Black Omen 1F Stairway",
			"{138} Black Omen 3F Walkway",
			"{139} Black Omen 47F Auxilary Command",
			"{13A} Black Omen 47F Grand Hall",
			"{13B} Black Omen 47F Emporium",
			"{13C} Black Omen 47F Royal Path",
			"{13D} Black Omen 47F Royal Ballroom",
			"{13E} Black Omen 47F Royal Assembly",
			"{13F} Black Omen 47F Royal Promenade",
			"{140} Black Omen Royal Teleporter (Lower)",
			"{141} Black Omen Royal Teleporter (Upper)",
			"{142} Black Omen 63F Divine Esplenade",
			"{143} Black Omen 63F Divine Guardian",
			"{144} Black Omen 97F Astral Walkway",
			"{145} Black Omen 98F Astral Guardian",
			"{146} Black Omen 98F Astral Walkway",
			"{147} Sunkeep (Prehistoric)",
			"{148} Zeal Palace Schala's Room",
			"{149} Zeal Palace Regal Hall",
			"{14A} Zeal Palace Corridor to the Mammon Machine",
			"{14B} Zeal Palace Hall of the Mammon Machine",
			"{14C} Zeal Palace Zeal Throneroom",
			"{14D} Zeal Palace Hall of the Mammon Machine (Night)",
			"{14E} Zeal Palace Zeal Throneroom (Night)",
			"{14F} Enhasa (wrong map)",
			"{150} Land Bridge (TBD)",
			"{151} Skyway? (TBD)",
			"{152} Present Age Home Room (Ending: The Dream Project)",
			"{153} Prehistoric Age Jungle (Ending: The Dream Project)",
			"{154} Dark Age Ocean Palace Room (Ending: The Dream Project)",
			"{155} Present Age Jail Cell (Ending: The Dream Project)",
			"{156} Dark Age Earthbound Room (Ending: The Dream Project)",
			"{157} Present Age Northern Ruins Room (Ending: The Dream Project)",
			"{158} Arris Dome Food Locker",
			"{159} Lucca's Workshop (Ending: The Oath)",
			"{15A} Arris Dome Guardian Chamber (Battle with Lavos)",
			"{15B} Prison Catwalks (Battle with Lavos)",
			"{15C} Heckran Cave (Battle with Lavos)",
			"{15D} Zenan Bridge (Battle with Lavos)",
			"{15E} Cave of the Masamune (Battle with Lavos)",
			"{15F} Dark Ages Portal",
			"{160} Land Bridge (TBD)",
			"{161} Land Bridge (TBD)",
			"{162} Skyway (TBD)",
			"{163} Enhasa",
			"{164} Skyway (TBD)",
			"{165} Kajar",
			"{166} Kajar Study",
			"{167} Kajar Belthasar's Private Study",
			"{168} Kajar Magic Lab",
			"{169} Enhasa Belthasar's Private Study",
			"{16A} Blackbird Scaffolding",
			"{16B} Blackbird Left Wing",
			"{16C} Blackbird Right Port",
			"{16D} Blackbird Left Port",
			"{16E} Blackbird Overhead",
			"{16F} Blackbird Hanger",
			"{170} Blackbird Rear Halls",
			"{171} Blackbird Forward Halls",
			"{172} Blackbird Treasury",
			"{173} Blackbird Cell",
			"{174} Blackbird Barracks",
			"{175} Blackbird Armory 3",
			"{176} Blackbird Inventory",
			"{177} Blackbird Lounge",
			"{178} Blackbird Ducts",
			"{179} Reborn Epoch",
			"{17A} Future Age Room (Ending: The Dream Project)",
			"{17B} End of Time (Ending: The Dream Project)",
			"{17C} Algetty",
			"{17D} Algetty Inn",
			"{17E} Algetty Elder's Grotto",
			"{17F} Algetty Commoner Grotto",
			"{180} Algetty Shop",
			"{181} Algetty Tsunami (wrong map)",
			"{182} Algetty Entrance",
			"{183} The Beast's Nest (wrong map)",
			"{184} The Beast's Nest",
			"{185} Zeal Teleporters",
			"{186} Prehistoric Hut (Ending: The Dream Project)",
			"{187} Castle Magus Room (Ending: The Dream Project)",
			"{188} Mt. Woe Western Face",
			"{189} Mt. Woe Lower Eastern Face",
			"{18A} Mt. Woe Middle Eastern Face",
			"{18B} Mt. Woe Upper Eastern Face",
			"{18C} Mt. Woe Summit (wrong map)",
			"{18D} Mt. Woe Summit",
			"{18E} Leene Square (Ending: What the Prophet Seeks)",
			"{18F} Crono's Kitchen (Ending: What the Prophet Seeks)",
			"{190} The End (Ending: Multiple?)",
			"{191} Zeal Palace",
			"{192} Zeal Palace Hallway",
			"{193} Zeal Palace Study",
			"{194} Ocean Palace Entrance",
			"{195} Ocean Palace Piazza",
			"{196} Ocean Palace Side Rooms",
			"{197} Ocean Palace Forward Area",
			"{198} Ocean Palace B3 Landing",
			"{199} Ocean Palace Grand Stairwell",
			"{19A} Ocean Palace B20 Landing",
			"{19B} Ocean Palace Southern Access Lift",
			"{19C} Ocean Palace Security Pool",
			"{19D} Ocean Palace Security Esplanade",
			"{19E} Ocean Palace Regal Antechamber",
			"{19F} Ocean Palace Throneroom",
			"{1A0} Ocean Palace (TBD)",
			"{1A1} Ocean Palace Eastern Access Lift",
			"{1A2} Ocean Palace Western Access Lift",
			"{1A3} Ocean Palace Time Freeze (wrong map)",
			"{1A4} Ocean Palace Time Freeze (wrong map)",
			"{1A5} Ocean Palace Time Freeze (wrong map)",
			"{1A6} Time Distortion, The Profane Machine",
			"{1A7} Ocean Palace Time Freeze",
			"{1A8} Last Village Commons",
			"{1A9} Last Village Empty Hut",
			"{1AA} Last Village Shop",
			"{1AB} Last Village Residence",
			"{1AC} North Cape",
			"{1AD} Death Peak Summit",
			"{1AE} Tyrano Lair Main Cell",
			"{1AF} Title Screen (wrong map)",
			"{1B0} Flying Epoch",
			"{1B1} Title Screen",
			"{1B2} Bekkler's Lab",
			"{1B3} Magic Cave Exterior (after cutscene)",
			"{1B4} Fiona's Forest Campfire",
			"{1B5} Factory Ruins Robot Storage",
			"{1B6} Courtroom King's Trial",
			"{1B7} Leene Square",
			"{1B8} Guardia Rear Storage",
			"{1B9} Courtroom Lobby",
			"{1BA} Blackbird Access Shaft",
			"{1BB} Blackbird Armory 2",
			"{1BC} Blackbird Armory 1",
			"{1BD} Blackbird Storage",
			"{1BE} Castle Magus Upper Battlements",
			"{1BF} Castle Magus Grand Stairway",
			"{1C0} (Bad Event Data Packet)",
			"{1C1} Black Omen Entrance",
			"{1C2} Black Omen 98F Omega Defense",
			"{1C3} Black Omen 99F Seat of Agelessness",
			"{1C4} (Bad Event Data Packet)",
			"{1C5} (Bad Event Data Packet)",
			"{1C6} (Bad Event Data Packet)",
			"{1C7} (Bad Event Data Packet)",
			"{1C8} Reptite Lair (Battle with Lavos)",
			"{1C9} Castle Magus Inner Sanctum (Battle with Lavos)",
			"{1CA} Tyrano Lair Keep (Battle with Lavos)",
			"{1CB} Mt. Woe Summit (Battle with Lavos)",
			"{1CC} Credits (Ending: The Oath)",
			"{1CD} (Empty Data)",
			"{1CE} (Empty Data)",
			"{1CF} (Empty Data)",
			"{1D0} End of Time",
			"{1D1} Spekkio",
			"{1D2} Apocalypse Lavos",
			"{1D3} Lavos",
			"{1D4} Guardia Queen's Tower (Middle Ages)",
			"{1D5} Castle Magus Corridor of Combat",
			"{1D6} Castle Magus Hall of Ambush",
			"{1D7} Castle Magus Dungeon",
			"{1D8} Apocalypse Epoch",
			"{1D9} End of Time Epoch",
			"{1DA} Lavos Tunnel",
			"{1DB} Lavos Core",
			"{1DC} Truce Dome",
			"{1DD} Emergence of the Black Omen",
			"{1DE} Blackbird Wing Access",
			"{1DF} Tesseract",
			"{1E0} Guardia King's Tower (Middle Ages)",
			"{1E1} Death of the Blackbird",
			"{1E2} Blackbird (no exits)",
			"{1E3} Blackbird (no exits)",
			"{1E4} Blackbird (no exits)",
			"{1E5} Blackbird (no exits)",
			"{1E6} Guardia King's Tower (Present)",
			"{1E7} Guardia Queen's Tower (Present)",
			"{1E8} Guardia Lawgiver's Tower",
			"{1E9} Guardia Prison Tower",
			"{1EA} Ancient Tyrano Lair Room of Vertigo",
			"{1EB} (empty map)",
			"{1EC} (empty map)",
			"{1ED} Algetty Tsunami",
			"{1EE} Paradise Lost",
			"{1EF} Death Peak Guardian Spawn",
			"{1F0} Present",
			"{1F1} Middle Ages",
			"{1F2} Future",
			"{1F3} Prehistoric",
			"{1F4} Dark Ages",
			"{1F5} Kingdom of Zeal",
			"{1F6} Last Village",
			"{1F7} Apocalypse",
			"{1F8} (empty map)",
			"{1F9} (empty map)",
			"{1FA} (empty map)",
			"{1FB} (empty map)",
			"{1FC} (empty map)",
			"{1FD} (empty map)",
			"{1FE} (empty map)",
			"{1FF} (empty map)"
		};
		public static string[] sBetaLocation = new string[512];
		public static string[] sOverworld = new string[8];
		public static string[] sDefOverworld = {
			"{0} Present",
			"{1} Middle Ages",
			"{2} Future",
			"{3} Prehistoric",
			"{4} Dark Ages",
			"{5} Kingdom of Zeal",
			"{6} Last Village",
			"{7} Apocalypse"
		};
		public static string[] sCardDir = {
			"Up",
			"Down",
			"Left",
			"Right"
		};
		public static string[] sPC = {
			"Crono",
			"Marle",
			"Lucca",
			"Robo",
			"Frog",
			"Ayla",
			"Magus",
			"Epoch"
		};
		public static string[] sNPC = {
			"{00} Melchior",
			"{01} King Guardia XXXIII [1000 A.D.]",
			"{02} Johnny",
			"{03} Queen Leene",
			"{04} Tata",
			"{05} Toma",
			"{06} Kino",
			"{07} Chancellor (green) [1000 A.D.]",
			"{08} Dactyl",
			"{09} Schala",
			"{0A} Janus",
			"{0B} Chancellor (brown) [600 A.D.]",
			"{0C} Belthasar",
			"{0D} Middle Ages/Present Age villager - woman",
			"{0E} Middle Ages/Present Age villager - young man",
			"{0F} Middle Ages/Present Age villager - young woman",
			"{10} Middle Ages/Present Age villager - soldier",
			"{11} Middle Ages/Present Age villager - old man",
			"{12} Middle Ages/Present Age villager - old woman",
			"{13} Middle Ages/Present Age villager - little boy",
			"{14} Middle Ages/Present Age villager - little girl",
			"{15} Middle Ages/Present Age villager - waitress",
			"{16} Middle Ages/Present Age villager - shopkeeper",
			"{17} Nun",
			"{18} Knight captain [600 A.D.]",
			"{19} Middle Ages/Present Age villager - man",
			"{1A} Dome survivor - man",
			"{1B} Dome survivor - woman",
			"{1C} Doan",
			"{1D} Dome survivor - little girl",
			"{1E} Prehistoric villager - man with club",
			"{1F} Prehistoric villager - woman in green dress",
			"{20} Prehistoric villager - little girl",
			"{21} Prehistoric villager - old man",
			"{22} Zeal citizen - man",
			"{23} Zeal citizen - woman",
			"{24} Zeal citizen - researcher with glasses",
			"{25} Crono's mom",
			"{26} Middle Ages/Present Age villager - little girl with purple hair",
			"{27} Middle Ages/Present Age villager - man",
			"{28} Middle Ages/Present Age villager - woman with purple hair",
			"{29} Middle Ages/Present Age villager - young man",
			"{2A} Middle Ages/Present Age villager - young woman",
			"{2B} Middle Ages/Present Age villager - soldier",
			"{2C} Middle Ages/Present Age villager - old man",
			"{2D} Middle Ages/Present Age villager - old woman",
			"{2E} Middle Ages/Present Age villager - little boy",
			"{2F} Middle Ages/Present Age villager - little girl",
			"{30} Middle Ages/Present Age villager - waitress with purple hair",
			"{31} Middle Ages/Present Age villager - shopkeeper",
			"{32} Nun",
			"{33} Guardia knight [600 A.D.]",
			"{34} Middle Ages/Present Age villager - man",
			"{35} Cyrus",
			"{36} Young Glenn",
			"{37} King Guardia XXI [600 A.D.]",
			"{38} Strength Test Machine part (Millennial Fair)",
			"{39} Middle Ages/Present Age villager - old man (2C dupe/UNUSED)",
			"{3A} Zeal citizen - researcher with glasses",
			"{3B} Cat",
			"{3C} False prophet Magus",
			"{3D} Melchior in gray robe/UNUSED",
			"{3E} Prehistoric villager - man carrying club with purple hair",
			"{3F} Prehistoric villager - woman with purple hair",
			"{40} Prehistoric villager - little girl with purple hair",
			"{41} Algetty earthbound one - man",
			"{42} Algetty earthbound one - woman",
			"{43} Algetty earthbound one - old man",
			"{44} Algetty earthbound one - child",
			"{45} Princess Nadia",
			"{46} Guardia Castle chef",
			"{47} Trial judge",
			"{48} Gaspar",
			"{49} Fiona",
			"{4A} Queen Zeal",
			"{4B} Guard (enemy)",
			"{4C} Reptite",
			"{4D} Kilwala",
			"{4E} Blue imp",
			"{4F} Middle Ages/Present Age villager - man",
			"{50} Middle Ages/Present Age villager - woman",
			"{51} G.I. Jogger",
			"{52} Millennial Fair visitor - old man",
			"{53} Millennial Fair visitor - woman",
			"{54} Millennial Fair visitor - little boy",
			"{55} Millennial Fair visitor - little girl",
			"{56} Lightning bolt",
			"{57} Opened time portal - upper half",
			"{58} Opened time portal - lower half",
			"{59} Millennial Fair shopkeeper",
			"{5A} Guillotine blade",
			"{5B} Guillotine chain",
			"{5C} Conveyor machine",
			"{5D} Tombstone",
			"{5E} Giant soup bowl",
			"{5F} Magus statue",
			"{60} Dreamstone",
			"{61} Gate Key",
			"{62} Soda can",
			"{63} Pendant",
			"{64} Poyozo doll",
			"{65} Pink lunch bag",
			"{66} UNUSED",
			"{67} Red knife ",
			"{68} Broken Masamune blade",
			"{69} Slice of cake",
			"{6A} Trash can on its side",
			"{6B} Piece of cheese",
			"{6C} Barrel",
			"{6D} UNUSED",
			"{6E} Dead sunstone",
			"{6F} Metal mug",
			"{70} Blue star",
			"{71} Giant blue star",
			"{72} Red flame",
			"{73} Giant red flame",
			"{74} Explosion ball",
			"{75} Giant explosion ball",
			"{76} Smoke trail/UNUSED",
			"{77} Hero's medal",
			"{78} Balcony shadow",
			"{79} Save point",
			"{7A} Prehistoric villager - drummer",
			"{7B} Prehistoric villager - log drummer",
			"{7C} White explosion outline",
			"{7D} Leene's bell",
			"{7E} Bat hanging upside down",
			"{7F} Computer screen",
			"{80} Water splash",
			"{81} Explosion",
			"{82} Robo power-up sparks",
			"{83} Leaves falling/UNUSED",
			"{84} 3 coins spinning/UNUSED",
			"{85} Hole in the ground",
			"{86} Cooking smoke",
			"{87} 3 small explosion clouds",
			"{88} Wind element spinning",
			"{89} Water element/UNUSED",
			"{8A} Dirt mound",
			"{8B} Masamune spinning",
			"{8C} Music note/UNUSED",
			"{8D} Small fish/UNUSED",
			"{8E} Water splash/UNUSED",
			"{8F} Lightning bolt/UNUSED",
			"{90} Unknown graphics/UNUSED",
			"{91} UNUSED",
			"{92} Small rock",
			"{93} Small rock *92 duplicate/UNUSED*",
			"{94} Small rock *92 duplicate/UNUSED*",
			"{95} Rainbow shell",
			"{96} Shadow (beds)",
			"{97} Closed portal",
			"{98} Balloon/UNUSED",
			"{99} Light green bush",
			"{9A} Shadow on the ground/UNUSED",
			"{9B} Brown dreamstone/UNUSED",
			"{9C} Crane machine",
			"{9D} UNUSED",
			"{9E} Dripping water/UNUSED",
			"{9F} Cupboard doors",
			"{A0} Brown stones/UNUSED",
			"{A1} Dark green bush",
			"{A2} Journal",
			"{A3} Norstein Bekkler",
			"{A4} Rat",
			"{A5} Sparks from guillotine blade",
			"{A6} Zeal teleporter",
			"{A7} Ocean palace teleporter",
			"{A8} Truce Dome director",
			"{A9} Epoch seats",
			"{AA} Robot",
			"{AB} Red star",
			"{AC} Sealed portal",
			"{AD} Animated Zz (sleeping) icon",
			"{AE} Flying map Epoch",
			"{AF} Gray cat",
			"{B0} Yellow cat",
			"{B1} Alfador",
			"{B2} Time egg",
			"{B3} Zeal citizen - man (cast ending)",
			"{B4} Zeal citizen - woman",
			"{B5} Potted plant",
			"{B6} Kid with purple hair (Glenn/Cyrus cutscene)",
			"{B7} Sealed chest",
			"{B8} Squirrel (Programmer's Ending)",
			"{B9} Blue poyozo",
			"{BA} Stone rubble pile",
			"{BB} Rusted Robo",
			"{BC} Gaspar (Gurus cutscene)",
			"{BD} UNUSED",
			"{BE} Orange cat",
			"{BF} Middle Ages/Present Age villager - little boy",
			"{C0} Middle Ages/Present Age villager - little girl",
			"{C1} Spinning water element",
			"{C2} Blue shining star - small",
			"{C3} Blue shining star - large",
			"{C4} Multiple balloons",
			"{C5} Dancing woman (Millennial Fair ending)",
			"{C6} Millennial Fair visitor - little girl",
			"{C7} Silver Leene's bell",
			"{C8} Figure atop Magus' Castle (ending)",
			"{C9} Serving tray with drinks",
			"{CA} THE END text",
			"{CB} Human Glenn (ending cutscene)",
			"{CC} Queen Zeal (Death Peak cutscene)",
			"{CD} Schala (Death Peak cutscene)",
			"{CE} Lavos (Death Peak cutscene)",
			"{CF} Crono (Death Peak cutscene)",
			"{D0} Hironobu Sakaguchi (Programmer's Ending)",
			"{D1} Yuji Horii (Programmer's Ending)",
			"{D2} Akira Toriyama (Programmer's Ending)",
			"{D3} Kazuhiko Aoki (Programmer's Ending)",
			"{D4} Lightning flash",
			"{D5} Lara",
			"{D6} Purple explosion",
			"{D7} Crono's mom (Millennial Fair ending)",
			"{D8} UNUSED",
			"{D9} UNUSED",
			"{DA} UNUSED",
			"{DB} UNUSED",
			"{DC} UNUSED",
			"{DD} UNUSED",
			"{DE} UNUSED",
			"{DF} UNUSED",
			"{E0} Green balloon/UNUSED",
			"{E1} Yellow balloon/UNUSED",
			"{E2} Blue balloon/UNUSED",
			"{E3} Pink balloon/UNUSED",
			"{E4} Brown glowing light/UNUSED",
			"{E5} Yellow glowing light/UNUSED",
			"{E6} Purple glowing light/UNUSED",
			"{E7} Blue glowing light",
			"{E8} UNUSED",
			"{E9} UNUSED",
			"{EA} UNUSED",
			"{EB} UNUSED",
			"{EC} UNUSED",
			"{ED} UNUSED",
			"{EE} UNUSED",
			"{EF} UNUSED",
			"{F0} UNUSED",
			"{F1} UNUSED",
			"{F2} UNUSED",
			"{F3} UNUSED",
			"{F4} UNUSED",
			"{F5} UNUSED",
			"{F6} UNUSED",
			"{F7} UNUSED",
			"{F8} UNUSED",
			"{F9} UNUSED",
			"{FA} UNUSED",
			"{FB} UNUSED",
			"{FC} UNUSED",
			"{FD} UNUSED",
			"{FE} UNUSED",
			"{FF} UNUSED"
		};
		public static string[] sSoundEffect = {
			"{00} Cursor selection",
			"{01} Invalid cursor selection",
			"{02} Falling sprite",
			"{03} Pendant reacting",
			"{04} Received item",
			"{05} Activating portal",
			"{06} HP/MP restored",
			"{07} End of Time HP/MP restore bucket",
			"{08} Weapon readied",
			"{09} Pendant reacting to Zeal throne room door",
			"{0A} Flying object",
			"{0B} Save point",
			"{0C} UNUSED",
			"{0D} UNUSED",
			"{0E} UNUSED",
			"{0F} UNUSED",
			"{10} UNUSED",
			"{11} UNUSED",
			"{12} UNUSED",
			"{13} UNUSED",
			"{14} UNUSED",
			"{15} UNUSED",
			"{16} UNUSED",
			"{17} UNUSED",
			"{18} UNUSED",
			"{19} UNUSED",
			"{1A} UNUSED",
			"{1B} UNUSED",
			"{1C} UNUSED",
			"{1D} UNUSED",
			"{1E} UNUSED",
			"{1F} UNUSED",
			"{20} UNUSED",
			"{21} UNUSED",
			"{22} UNUSED",
			"{23} UNUSED",
			"{24} UNUSED",
			"{25} UNUSED",
			"{26} UNUSED",
			"{27} UNUSED",
			"{28} UNUSED",
			"{29} UNUSED",
			"{2A} UNUSED",
			"{2B} UNUSED",
			"{2C} UNUSED",
			"{2D} UNUSED",
			"{2E} UNUSED",
			"{2F} UNUSED",
			"{30} UNUSED",
			"{31} UNUSED",
			"{32} UNUSED",
			"{33} UNUSED",
			"{34} UNUSED",
			"{35} UNUSED",
			"{36} UNUSED",
			"{37} UNUSED",
			"{38} UNUSED",
			"{39} UNUSED",
			"{3A} UNUSED",
			"{3B} UNUSED",
			"{3C} UNUSED",
			"{3D} UNUSED",
			"{3E} UNUSED",
			"{3F} Nothing",
			"{40} Curtains",
			"{41} Wind [LOOPS]",
			"{42} Machine engine [LOOPS]",
			"{43} PC/NPC KO'd",
			"{44} Machine powering up [LOOPS]",
			"{45} Fireworks - REQUIRES SONG 11",
			"{46} Ocean waves [LOOPS]",
			"{47} Bats flying [LOOPS]",
			"{48} Frog catches Gold Rock",
			"{49} Long explosions",
			"{4A} Stomach growling",
			"{4B} Computer screen error [LOOPS]",
			"{4C} Ferry horn",
			"{4D} Enemy surprises party",
			"{4E} Cat meow",
			"{4F} Long fall",
			"{50} Heavy sirens [LOOPS]",
			"{51} Sealed door opening",
			"{52} Switch pressed",
			"{53} Door opened",
			"{54} Earthquake/rumbling [LOOPS]",
			"{55} Gold received",
			"{56} Giant doors opened",
			"{57} Metal mug put down",
			"{58} Unknown [LOOPS] (Unused)",
			"{59} Metal objects colliding 1 ",
			"{5A} Metal objects colliding 2",
			"{5B} Magic Urn enemy",
			"{5C} Exhaust",
			"{5D} Unknown [LOOPS] (Unused)",
			"{5E} Conveyor belt [LOOPS]",
			"{5F} Frog versus Magus battle",
			"{60} Metal gate crashing",
			"{61} Squeak",
			"{62} Running [LOOPS]",
			"{63} Weapon readied",
			"{64} Poly rolling [LOOPS]",
			"{65} Treasure chest opened",
			"{66} Carpenter's tools",
			"{67} Invalid password entry",
			"{68} Crane password prompt",
			"{69} Dactyl flying [LOOPS]",
			"{6A} Cup clang",
			"{6B} Evil laugh",
			"{6C} Machine malfunction [LOOPS]",
			"{6D} Elevator moving [LOOPS]",
			"{6E} Frog croak",
			"{6F} Enemy scream 1",
			"{70} Portal opening/closing",
			"{71} Moving machine [LOOPS]",
			"{72} Guardia Knight attack",
			"{73} Enemy scream 2",
			"{74} Pathway opens",
			"{75} Unknown (Unused) [LOOPS]",
			"{76} Unknown (Unused)",
			"{77} Big explosion",
			"{78} Teleport",
			"{79} Monster snarl",
			"{7A} NPC scream",
			"{7B} Lightning on 2300 A.D. map",
			"{7C} Thunder on 2300 A.D. map",
			"{7D} Ground cracking before Lavos battle",
			"{7E} Gavel strike - REQUIRES SONG 17",
			"{7F} Prisoner sigh",
			"{80} Rooster",
			"{81} Lucca shoots Guard",
			"{82} Metal bars rattling",
			"{83} Guard KO'd",
			"{84} Bushes/trees rustling",
			"{85} Forest Ruins pyramid breaking [LOOPS]",
			"{86} Telepod powering up [LOOPS]",
			"{87} Sword slice",
			"{88} Object powering up [LOOPS]",
			"{89} Transformation",
			"{8A} Unknown (Unused)",
			"{8B} Slice",
			"{8C} Crashing metal",
			"{8D} Sprite lands",
			"{8E} Collision",
			"{8F} Bat squeak",
			"{90} Enemy scream 3",
			"{91} Imp Ace flying",
			"{92} Dragon Tank moving [LOOPS]",
			"{93} Ghosts [LOOPS]",
			"{94} Sewer bridge extending",
			"{95} Giant doors rumbling",
			"{96} Bike race countdown",
			"{97} Countdown start signal",
			"{98} Robot noise",
			"{99} Multiple explosions",
			"{9A} Explosion",
			"{9B} Ringing [LOOPS]",
			"{9C} Enertron",
			"{9D} Bones rattling",
			"{9E} Computer display power on",
			"{9F} Computer display power off",
			"{A0} Typing [LOOPS]",
			"{A1} Light sirens [LOOPS]",
			"{A2} Retinite moving [LOOPS]",
			"{A3} Orb enemy blinking",
			"{A4} Enemy scream 4",
			"{A5} Trial audience cheers",
			"{A6} Trial audience boos",
			"{A7} Enemy sleeping [LOOPS]",
			"{A8} Pop",
			"{A9} Powerful sword swing",
			"{AA} Enemy startled",
			"{AB} Water splash",
			"{AC} Epoch preparing to warp",
			"{AD} Epoch time warps",
			"{AE} Epoch powering down",
			"{AF} Epoch powering up 1 [LOOPS]",
			"{B0} Tonic obtained",
			"{B1} Laughing",
			"{B2} Lavos spawn scream",
			"{B3} Crono obtains magic",
			"{B4} Soldier walking [LOOPS]",
			"{B5} Parried attack",
			"{B6} Scout enemies appear",
			"{B7} Hyper Kabob cooking",
			"{B8} Digging",
			"{B9} Power Stew bubbling",
			"{BA} Screen wipe",
			"{BB} Machinery [LOOPS]",
			"{BC} Ozzie's barrier shattering",
			"{BD} Ozzie falling",
			"{BE} Masamune [LOOPS]",
			"{BF} Masamune light beam [LOOPS]",
			"{C0} Crane chain [LOOPS]",
			"{C1} Unknown (Unused)",
			"{C2} Magic Cave splits",
			"{C3} Keeper's Dome Epoch warp [LOOP]",
			"{C4} Robot computing 1",
			"{C5} Tyrano roar",
			"{C6} Robot computing 2",
			"{C7} Robot computing slow",
			"{C8} Epoch time warp",
			"{C9} Teleport",
			"{CA} Soda can bouncing",
			"{CB} Blackbird cargo door opening  [LOOPS]",
			"{CC} Blackbird cargo door opened",
			"{CD} Dinosaur skull opening",
			"{CE} Epoch firing laser[LOOPS]",
			"{CF} Rapid explosions [LOOPS]",
			"{D0} Epoch powering up 2 [LOOPS]",
			"{D1} Wormhole warp [LOOPS]",
			"{D2} Epoch laser damaging Blackbird [LOOPS]",
			"{D3} Robot computing 3",
			"{D4} Large splash",
			"{D5} Lavos beams destroying Zeal",
			"{D6} Dinner chime",
			"{D7} Power roast meal being prepared",
			"{D8} Lavos breathing [LOOPS]",
			"{D9} Epoch preparing to fly at Lavos [LOOPS]",
			"{DA} Epoch flying into Lavos (Mode7, 3rd person view) [LOOPS]",
			"{DB} Epoch flying into Lavos (Mode7, 1st person view) [LOOPS]",
			"{DC} Epoch crashes into Lavos",
			"{DD} Octo enemy",
			"{DE} UNUSED",
			"{DF} Light beams",
			"{E0} Top of Black Omen [LOOPS]",
			"{E1} Mammon Machine [LOOPS]",
			"{E2} Lavos 2nd form [LOOPS]",
			"{E3} Lavos 2nd form defeated",
			"{E4} Unknown  (Unused)",
			"{E5} Explosion engulfing Black Omen",
			"{E6} Lavos eruption explosion",
			"{E7} Computer analyzing map 1",
			"{E8} Computer analyzing map 2 [LOOPS]",
			"{E9} Ending slideshow [LOOPS]",
			"{EA} UNUSED",
			"{EB} UNUSED",
			"{EC} UNUSED",
			"{ED} UNUSED",
			"{EE} UNUSED",
			"{EF} UNUSED",
			"{F0} UNUSED",
			"{F1} UNUSED",
			"{F2} UNUSED",
			"{F3} UNUSED",
			"{F4} Unknown (Unused)",
			"{F5} UNUSED",
			"{F6} UNUSED",
			"{F7} UNUSED",
			"{F8} UNUSED",
			"{F9} UNUSED",
			"{FA} UNUSED",
			"{FB} UNUSED",
			"{FC} UNUSED",
			"{FD} UNUSED",
			"{FE} Unknown (Unused) [LOOPS]",
			"{FF} [END LOOP]"
		};
		public static string[] sStore = {
			"Store 00 - Melchior (Millennial Fair)",
			"Store 01 - Truce Market (Middle Ages)",
			"Store 02 - Truce Market (Present)",
			"Store 03 - Arris Dome",
			"Store 04 - Melchior's Hut, Truce Market (Present, Updated)",
			"Store 05 - Sandorino Market",
			"Store 06 - Porre Market (Middle Ages)",
			"Store 07 - Last Village",
			"Store 08 - Ioka Trading Post",
			"Store 09 - Last Village (Updated)",
			"Store 0A - Kajar",
			"Store 0B - Enhasa",
			"Store 0C - Porre Market (Present)",
			"Store 0D - Algetty",
			"Store 0E - Choras Inn (Present)",
			"Store 0F - Choras Market (Middle Ages)",
			"Store 10 - Millennial Fair Armor",
			"Store 11 - Millennial Fair Items",
			"Store 12 - UNUSED (empty)",
			"Store 13 - Trann Dome",
			"Store 14 - UNUSED",
			"Store 15 - Medina Market",
			"Store 16 - Fiona's Shrine",
			"Store 17 - Black Omen"
		};
		public static string[] sColorMathColors = {
			"Off / Black",
			"Red",
			"Lime",
			"Yellow",
			"Blue",
			"Fuchsia",
			"Aqua",
			"White"
		};
		public static List<uint[]> nRomAddr;
		public static List<ushort[]> nRomValue;
		public static byte nRomType = 1;
		public static byte[] WorkingData;
		public static cFreeSpace FreeSpace;
		public static byte nZoomFactor = 1;
		//public static DockPanel DockMan;
		//public static StatusBarPanel CoordStatus;
		public static Dictionary<uint, string> KnownAddrHash;
		public static PostStatusDel PostStatus;
		public static GetStrFromGroupDel GetStrFromGroup;

		public static string KnownAddr(uint nAddr)
		{
			if (((int) nAddr & 0x7E0000) == 0)
				nAddr |= 0x7E0000;
			return "Mem." + KnownAddrHash.GetValueOrDefault(nAddr, HexStr(nAddr, 6));
		}

		public static uint GetFileOffset(uint[] nData) => GetFileOffset((uint) (WorkingData[nData[0]] << 16 | WorkingData[nData[1]] << 8) | WorkingData[nData[2]]);

		public static uint GetFileOffset(int nAddrOffset) => GetFileOffset(GetInt24(WorkingData, nAddrOffset));

		public static uint GetRomAddr(RomAddr RomAddress) => nRomAddr[(int) RomAddress][nRomType];

		public static ushort GetRomValue(RomValue RomVal) => nRomValue[(int) RomVal][nRomType];
	}
}
