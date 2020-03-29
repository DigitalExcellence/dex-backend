using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    IdentityId = table.Column<string>(nullable: false),
                    ProfileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ShortDescription = table.Column<string>(nullable: false),
                    Uri = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Collaborators",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collaborators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Collaborators_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "IdentityId", "Name", "ProfileUrl" },
                values: new object[,]
                {
                    { 1, "Owen.Welch@hotmail.com", "2", "Lelah", null },
                    { 28, "Devonte_Bashirian30@hotmail.com", "29", "Mathew", null },
                    { 27, "Estella12@yahoo.com", "28", "Marilie", null },
                    { 26, "Lacy.Lindgren43@gmail.com", "27", "Leanna", null },
                    { 25, "Vada.Schmidt54@hotmail.com", "26", "Kiera", null },
                    { 24, "Trycia65@hotmail.com", "25", "Alberta", null },
                    { 23, "Freeda_White80@gmail.com", "24", "Macey", null },
                    { 22, "Eden_Hudson11@yahoo.com", "23", "Viola", null },
                    { 21, "Kiana_Carroll@gmail.com", "22", "Melba", null },
                    { 20, "Maddison_Sporer77@yahoo.com", "21", "Gia", null },
                    { 19, "Edgardo_VonRueden64@yahoo.com", "20", "Pattie", null },
                    { 18, "Adalberto64@gmail.com", "19", "Marietta", null },
                    { 17, "Emelia_Hickle@hotmail.com", "18", "Jace", null },
                    { 16, "Jadyn35@hotmail.com", "17", "Delphia", null },
                    { 15, "Ivy22@hotmail.com", "16", "Roy", null },
                    { 14, "Jeanie.Kilback95@gmail.com", "15", "Ashleigh", null },
                    { 13, "Ashleigh_Bailey6@hotmail.com", "14", "Vance", null },
                    { 12, "Lacy82@gmail.com", "13", "Rebecca", null },
                    { 11, "Claudine_Torphy@gmail.com", "12", "Edyth", null },
                    { 10, "Flo85@yahoo.com", "11", "Marcel", null },
                    { 9, "Zoila_Watsica39@hotmail.com", "10", "Vincenza", null },
                    { 8, "Angeline63@gmail.com", "9", "Dena", null },
                    { 7, "Koby_Gaylord30@yahoo.com", "8", "Emery", null },
                    { 6, "Taryn12@yahoo.com", "7", "Queenie", null },
                    { 5, "Howell.Erdman24@hotmail.com", "6", "Vaughn", null },
                    { 4, "Ari.Oberbrunner27@hotmail.com", "5", "Madisyn", null },
                    { 3, "Junior_Carroll@yahoo.com", "4", "Elinore", null },
                    { 2, "Karelle99@gmail.com", "3", "Paige", null },
                    { 29, "Alexa9@hotmail.com", "30", "Adan", null },
                    { 30, "Eino_Jakubowski@gmail.com", "31", "Shania", null }
                });

            migrationBuilder.InsertData(
                table: "Project",
                columns: new[] { "Id", "Created", "Description", "Name", "ShortDescription", "Updated", "Uri", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 3, 27, 14, 30, 11, 864, DateTimeKind.Local).AddTicks(2463), "edfqtleateaiirtiesiuaiouiumoaortdintiumheeanlcoeeociselseseeuvicrstalsuostqamqaqtapiinqgissqttsnstqitlebtiusmunneosqmtfvmttlnouasnsottloodrosaunsuotcs", "Awesome Granite Bacon", "ssomsiileeressepaeslqvetutlril", new DateTime(2020, 3, 29, 14, 30, 11, 866, DateTimeKind.Local).AddTicks(9821), "http://bret.name", 1 },
                    { 25, new DateTime(2020, 3, 27, 14, 30, 11, 933, DateTimeKind.Local).AddTicks(2661), "mtdaeseedteouseiteefmsqtpioqsddiitvnlisuortgdeleqeutdntaeaeuicnmauerrottieutaeepbeteicecnretutiiorpatitsuueetncaxermeuuetisotcbnieuriuenmtcteeitneauit", "Incredible Frozen Gloves", "aqurautblpetealdaaeitcoouesiou", new DateTime(2020, 3, 29, 14, 30, 11, 933, DateTimeKind.Local).AddTicks(2690), "https://presley.biz", 24 },
                    { 21, new DateTime(2020, 3, 27, 14, 30, 11, 922, DateTimeKind.Local).AddTicks(4990), "saimqenuuseuaievmpsmimcrdrattareaqtiamutpaeupeeaiictteletsuetitsaiqnrdsomsnupuadtioslepauepaeeirsusiloamtuevnnlquisutouasqtsonauqidteoepraaedssuitulia", "Handmade Rubber Chair", "iciseuaesliitmosesreamsunutipr", new DateTime(2020, 3, 29, 14, 30, 11, 922, DateTimeKind.Local).AddTicks(5027), "http://luz.info", 24 },
                    { 5, new DateTime(2020, 3, 27, 14, 30, 11, 878, DateTimeKind.Local).AddTicks(969), "oeedtmfiaqaieauulamuanievmemntodtqramtuncoqaqutpltiaiisvgnnpoiaireotetarameeseertqiuoiipisqelncnoliortiasudttiitsuefimmesqorprtepueeoaeigeisumiqeepeuu", "Refined Cotton Pants", "ltgttlaraaioitamoidremieciicil", new DateTime(2020, 3, 29, 14, 30, 11, 878, DateTimeKind.Local).AddTicks(997), "https://dennis.com", 23 },
                    { 22, new DateTime(2020, 3, 27, 14, 30, 11, 925, DateTimeKind.Local).AddTicks(1403), "sqtloeeeasdetnnrsguaoousroeuitmaulstttmteduauliuuqebviialcesbtteiimxirmepteoamqdpstaeliniieuededoquarpciumrueutueeleeriltctieasqtusoaeesetuvseuiigttiq", "Ergonomic Frozen Chips", "metuvnsuueottuuisiiaoqirsemuou", new DateTime(2020, 3, 29, 14, 30, 11, 925, DateTimeKind.Local).AddTicks(1438), "https://mollie.name", 21 },
                    { 26, new DateTime(2020, 3, 27, 14, 30, 11, 936, DateTimeKind.Local).AddTicks(709), "atuiurisduqieaethpteruocaumriueiaainrulnmteameintlssigguteiteeamuousamnserndnptarsuauturcualaidriaosqitadtteepilusaldtoctmlisusqenauesaacmuaoetnneetme", "Intelligent Soft Shirt", "mimsniiaieiqepamrsssuimqnsentq", new DateTime(2020, 3, 29, 14, 30, 11, 936, DateTimeKind.Local).AddTicks(748), "https://margie.org", 20 },
                    { 20, new DateTime(2020, 3, 27, 14, 30, 11, 919, DateTimeKind.Local).AddTicks(6388), "dtsuatiiauunpueoemutipussassoicvunlqsutlsentiuusdvtolsqmeumsvuuanctanisiuiiqtlsctliimmmuiscitiqioutuctdteleuamaeotiliaimeaaaeeueeavesqoututtunrimmaiuu", "Refined Plastic Computer", "acutuuuqluelelaiisdludtuecegme", new DateTime(2020, 3, 29, 14, 30, 11, 919, DateTimeKind.Local).AddTicks(6420), "http://edd.org", 20 },
                    { 12, new DateTime(2020, 3, 27, 14, 30, 11, 897, DateTimeKind.Local).AddTicks(7509), "taeoueseemuaqxaditlsatratauiltmnpsottcoensptdutobteuueceeiiperedauumpltistulmttueadeisorrllusaiiplsmuamoilqaiiuituuotsvoetouraurltpmiedsoetaoptieaeenn", "Ergonomic Metal Car", "qitnmgrusitqqoasssutrpamdttiiu", new DateTime(2020, 3, 29, 14, 30, 11, 897, DateTimeKind.Local).AddTicks(7537), "https://alanis.net", 20 },
                    { 29, new DateTime(2020, 3, 27, 14, 30, 11, 944, DateTimeKind.Local).AddTicks(260), "eqaeindiilamenrshudaternemnrmaeilgucolurruergtatburqeucebrtiieecpcrelmtademiotesnaeocstdsinitguexeedqnoruelaotnsoeldnttaraluisoenuvahtrtelsuimqdardirr", "Generic Rubber Pants", "ssiaqeaaurulmraoqrdniaeolicuim", new DateTime(2020, 3, 29, 14, 30, 11, 944, DateTimeKind.Local).AddTicks(287), "https://jedediah.org", 18 },
                    { 23, new DateTime(2020, 3, 27, 14, 30, 11, 927, DateTimeKind.Local).AddTicks(6158), "slaefrsvvumiqaremuarqaasuthuoedliamocmdidettniiccaiioelllegmiilinmplanstsutusaephusiquitiumsulpppmeitesqeoltulumposnidoeefetqettaiuetlurdeuuacieneuarl", "Rustic Plastic Mouse", "oaiqupavuneiaartlrgmaeossuaqra", new DateTime(2020, 3, 29, 14, 30, 11, 927, DateTimeKind.Local).AddTicks(6194), "https://ellis.com", 18 },
                    { 6, new DateTime(2020, 3, 27, 14, 30, 11, 880, DateTimeKind.Local).AddTicks(8487), "ttesttotiillinrormsdotludmavalrssieqoocusiddmthtmuenleitiiuoitoovaestievtsioaoxiuaistmiuometaadaeivspaeiuiemaeaisumutuqiisuidtemsdantelsirnguetiietavi", "Tasty Steel Bacon", "uunrtlntomslutianmasuadtoeusno", new DateTime(2020, 3, 29, 14, 30, 11, 880, DateTimeKind.Local).AddTicks(8515), "http://zora.info", 17 },
                    { 30, new DateTime(2020, 3, 27, 14, 30, 11, 946, DateTimeKind.Local).AddTicks(3809), "oopeeoaarmrsaumiiisimeaqmteeidcimtauddilstisuvcuorndeuiueepfeoeiactdaueettnhmsiieoruoetssanedasneitqoutxgdisetaviteseuttsaiteiusettcomeabaeitueuceceaa", "Gorgeous Metal Shoes", "iqtltiitanrootetuedoemqugdteis", new DateTime(2020, 3, 29, 14, 30, 11, 946, DateTimeKind.Local).AddTicks(3833), "https://melba.name", 16 },
                    { 14, new DateTime(2020, 3, 27, 14, 30, 11, 903, DateTimeKind.Local).AddTicks(1443), "mslqrilstmaseldtuttdetiatasieeeeislintiriutitmifsmbunniomvuemraauqoucseutnnruiinvouselnnruuttniaidtspnetpelptiaadnaqidisisqtpelsuttdiamtilmtiieeaicato", "Incredible Concrete Shirt", "asuusramisnetiaeactqoeuqieasac", new DateTime(2020, 3, 29, 14, 30, 11, 903, DateTimeKind.Local).AddTicks(1480), "https://asia.biz", 16 },
                    { 16, new DateTime(2020, 3, 27, 14, 30, 11, 908, DateTimeKind.Local).AddTicks(7176), "amtdoaistsdpnoguelpuemctiomscgriaeamenaqrtsaecridnenvoaiuiertsstucetbiidtuofmosttuquueqeirloislonsaexnuduaenmsdounuautqauotalaituiasduaesnccaunssaaelo", "Tasty Granite Keyboard", "cidunoeettpeanretidieeeoideuii", new DateTime(2020, 3, 29, 14, 30, 11, 908, DateTimeKind.Local).AddTicks(7214), "https://kaylin.com", 15 },
                    { 3, new DateTime(2020, 3, 27, 14, 30, 11, 872, DateTimeKind.Local).AddTicks(6443), "tsmmmuleneqfeiqleueipspisiituitsersimltuuolelqetetmveebiiepgatiogdqcuitmteoeamavoaeuoeilearecudetneutddrcioitlxdremsnqeouxueuapiqqudeoapdilntqevimitoe", "Gorgeous Frozen Towels", "aoicbrorpqiustidtasiaavuallimb", new DateTime(2020, 3, 29, 14, 30, 11, 872, DateTimeKind.Local).AddTicks(6485), "https://virgie.biz", 14 },
                    { 10, new DateTime(2020, 3, 27, 14, 30, 11, 892, DateTimeKind.Local).AddTicks(2200), "eatbqateemauaeeriisashslenmperaliisptameaueuapiulhrsiituumnqfobmeeemuqseuguluueeesxaeaaimnandeeqingurseuloxoipihuprlmsftinedvitautotctqesmeieiapeaciut", "Practical Plastic Towels", "uldiaicoletrnotdoutrqqeeeenqnt", new DateTime(2020, 3, 29, 14, 30, 11, 892, DateTimeKind.Local).AddTicks(2239), "http://horace.biz", 13 },
                    { 4, new DateTime(2020, 3, 27, 14, 30, 11, 875, DateTimeKind.Local).AddTicks(2817), "iuriucelitctoqdtboipenniaustedeicpanlrqemrtlitsttsuotseaqameppslaeumeanimaincuqotorrsiduueactordseavepumleemqulitelcnqimaebiieeovoodisasbqnlqeurnliueo", "Licensed Rubber Shirt", "satamuttaseitticibnieseeoolttu", new DateTime(2020, 3, 29, 14, 30, 11, 875, DateTimeKind.Local).AddTicks(2859), "http://edwardo.net", 13 },
                    { 7, new DateTime(2020, 3, 27, 14, 30, 11, 883, DateTimeKind.Local).AddTicks(6561), "csiiiioumesudetieoreitqutmuoeoeupotidaittsumtsterqanrinutleqimofosaitliaolisspetnpusaetoieqceauqaemaisesaurtleveihsaeduesittumoeeaeeepimxentuituolalst", "Incredible Wooden Hat", "siuuetuacsedaarfoimoimsaaecpto", new DateTime(2020, 3, 29, 14, 30, 11, 883, DateTimeKind.Local).AddTicks(6615), "http://lexus.com", 11 },
                    { 18, new DateTime(2020, 3, 27, 14, 30, 11, 914, DateTimeKind.Local).AddTicks(1780), "amvucbiiqalolnissecedmomaauioretudamenqsuuudterauoltanilmaeexiaearueutsastlieaueaoumdttanuduunmvloeoqausrsiieidslueaeeaioaaaaiastttmmuetiusiteoimnepes", "Ergonomic Plastic Bacon", "bteeneevupqtnnbrnaiduoeprimime", new DateTime(2020, 3, 29, 14, 30, 11, 914, DateTimeKind.Local).AddTicks(1807), "https://buck.net", 10 },
                    { 8, new DateTime(2020, 3, 27, 14, 30, 11, 886, DateTimeKind.Local).AddTicks(7073), "aousvmotautausitsuriipuuaudqvpetpaecuqeenqurepusxilospmolaviporrnonuvtsbanrbeememtoiedmtseuochltiuespuvmauuagmeleemeseeutstunuuseoxmtommaasdtasnmiluta", "Awesome Granite Towels", "bdeteaiudioeatmetvnutoasiaiiua", new DateTime(2020, 3, 29, 14, 30, 11, 886, DateTimeKind.Local).AddTicks(7120), "http://casper.com", 10 },
                    { 2, new DateTime(2020, 3, 27, 14, 30, 11, 869, DateTimeKind.Local).AddTicks(8407), "esotaoeumeopaaueueeimmqusaetbauqqventcsatnluguamnedsisttfenseebtndenarouioiliseomsaalemltnielitbleslasmuogcnoumrelpmeuuuuualldeueurcmuseemmsieisiuosnu", "Refined Concrete Pizza", "ipqnseoioidniqsiirsueemieutmsu", new DateTime(2020, 3, 29, 14, 30, 11, 869, DateTimeKind.Local).AddTicks(8460), "http://kenna.org", 9 },
                    { 15, new DateTime(2020, 3, 27, 14, 30, 11, 905, DateTimeKind.Local).AddTicks(9552), "eiumienogemituqcueqiiimmeuqcommtteausosonnasardinacedsqufrcielrppatqeusiuimvtbuiuietcanlteniamrgoathimtsxuieouumlatoloroenuesdiligemumutlafrsumeuitmmo", "Small Plastic Table", "iiofqpmeamuvliaritiletcnodmtti", new DateTime(2020, 3, 29, 14, 30, 11, 905, DateTimeKind.Local).AddTicks(9591), "https://jerald.name", 7 },
                    { 13, new DateTime(2020, 3, 27, 14, 30, 11, 900, DateTimeKind.Local).AddTicks(4555), "saecdiuntaelruliinduiimifetinoqfcoeeiqoaoidteoeltddadaudiaputsmeiuuqiqtumstnnlestiviulcdmtueslreuitrneatllmidquomdqesetsptleelioniaensmhttutttiarciene", "Handcrafted Fresh Mouse", "mihmomiqqtdsnuuonitomcardotmre", new DateTime(2020, 3, 29, 14, 30, 11, 900, DateTimeKind.Local).AddTicks(4584), "http://johnnie.org", 6 },
                    { 24, new DateTime(2020, 3, 27, 14, 30, 11, 930, DateTimeKind.Local).AddTicks(3719), "imatsstaauopiqriuuiddsucueutnmteteurlevrbeqeeullxuilmonstitfdildclipsttdiitaoieiusetttenemmgitdeleimiieitolaataqeueeinuitueaoitvoeiesutaiaederqoqequsa", "Handmade Wooden Bacon", "pxretarusltciuqeiatraauisdtsas", new DateTime(2020, 3, 29, 14, 30, 11, 930, DateTimeKind.Local).AddTicks(3747), "http://imelda.org", 5 },
                    { 9, new DateTime(2020, 3, 27, 14, 30, 11, 889, DateTimeKind.Local).AddTicks(5125), "ubomseotematousmttaammutuienmoueaieesaiununuulqsvroursnmanutsrimmttfsneslaiqriiquiaaostlqqlesatiuosueomtiisahusequuitfluqeeiuuieieebsstnhvindanceodtli", "Awesome Frozen Computer", "tmsueieeneiaeotteetmiuueuleill", new DateTime(2020, 3, 29, 14, 30, 11, 889, DateTimeKind.Local).AddTicks(5157), "https://johann.net", 5 },
                    { 28, new DateTime(2020, 3, 27, 14, 30, 11, 941, DateTimeKind.Local).AddTicks(2691), "aaiudmetletietindoaiapeqmeidtnedtesaavotaultmoudtiipaacmiarlrserueisdrdreniatisrisitotnqiuvtqtteucruicoeiuisdiulamnroqasnotvoeudbnuniaseapoolalopuaovu", "Tasty Granite Gloves", "utedeoeqaratciseemotuiiitoeiau", new DateTime(2020, 3, 29, 14, 30, 11, 941, DateTimeKind.Local).AddTicks(2727), "http://lupe.com", 2 },
                    { 27, new DateTime(2020, 3, 27, 14, 30, 11, 938, DateTimeKind.Local).AddTicks(5495), "vbsrrmvuauuniicptauuapeurotaiasaprricthttataisvretnutqopaeaceposieuaudmccqvubteuutxtvirnddnuomiqmrenpeeterusutalltueiiesptaeueesrdituuluttentnueitauei", "Practical Soft Soap", "ilotiatlataclmiruiuuusirdqssru", new DateTime(2020, 3, 29, 14, 30, 11, 938, DateTimeKind.Local).AddTicks(5522), "http://roosevelt.net", 2 },
                    { 11, new DateTime(2020, 3, 27, 14, 30, 11, 895, DateTimeKind.Local).AddTicks(162), "oneodatoeidsetueeatunattemasamapionuiolmsiemmemdtaxuamscsrspvioatqtuiinibiosaulneeitaetltleeebssiicbpoietclmesausuttueadutumieliqioiiquisulmoammanelum", "Tasty Fresh Salad", "dcrinussaeluaulebmoseseteaiueu", new DateTime(2020, 3, 29, 14, 30, 11, 895, DateTimeKind.Local).AddTicks(193), "https://kaleigh.biz", 1 },
                    { 19, new DateTime(2020, 3, 27, 14, 30, 11, 916, DateTimeKind.Local).AddTicks(9166), "cnltdacivrteunamqnituqqmopohsrrtpenuvamtaoutmaauuoteinuuoeebiqlerttcttlenottuotdneutellstigloeieudioeauaqeatmotocramostpetrcnsiseutiuilmaqrmonlhmtseeu", "Rustic Plastic Car", "mmaeapussurtseepiqmtftaqeiadsc", new DateTime(2020, 3, 29, 14, 30, 11, 916, DateTimeKind.Local).AddTicks(9198), "http://tatyana.biz", 27 },
                    { 17, new DateTime(2020, 3, 27, 14, 30, 11, 911, DateTimeKind.Local).AddTicks(4883), "luaimuesomxetesqdqesurqeoioiemrtoraqalmeestrtsadmtqtuenusgummemldtiteperescciveietnscmquisaqeltunraaiqmiieutrprooiteltauomstmepiurmrsdeaieaeeuemppdeua", "Handmade Cotton Towels", "iusqunuaeteoqrutiittleassereei", new DateTime(2020, 3, 29, 14, 30, 11, 911, DateTimeKind.Local).AddTicks(4920), "https://baby.org", 29 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_ProjectId",
                table: "Collaborators",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_UserId",
                table: "Project",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Collaborators");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
