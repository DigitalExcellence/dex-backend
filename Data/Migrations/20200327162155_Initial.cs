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
                    IdentityId = table.Column<string>(nullable: false)
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
                    Contributors = table.Column<string>(nullable: true),
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

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "IdentityId", "Name" },
                values: new object[,]
                {
                    { 1, "Donna1@yahoo.com", "2", "Bernadine" },
                    { 28, "Ila.Stamm@hotmail.com", "29", "Kameron" },
                    { 27, "Aurelio.Rutherford@hotmail.com", "28", "Evalyn" },
                    { 26, "Cade_Miller@hotmail.com", "27", "Skyla" },
                    { 25, "Theodora61@hotmail.com", "26", "Johnathon" },
                    { 24, "Garett44@hotmail.com", "25", "Maximillia" },
                    { 23, "Lamont_Ebert@gmail.com", "24", "Luciano" },
                    { 22, "Major.Stamm87@hotmail.com", "23", "Hayden" },
                    { 21, "Horace.Olson@hotmail.com", "22", "Talon" },
                    { 20, "Timmy_OKeefe@hotmail.com", "21", "Zelda" },
                    { 19, "Orville10@yahoo.com", "20", "German" },
                    { 18, "Burdette69@yahoo.com", "19", "Abby" },
                    { 17, "Tyrese.Lang@yahoo.com", "18", "Kasey" },
                    { 16, "Casandra.Barrows54@gmail.com", "17", "Daniella" },
                    { 15, "Wendy_Bins@hotmail.com", "16", "Marjolaine" },
                    { 14, "Lolita90@gmail.com", "15", "Renee" },
                    { 13, "Maude_Howe@yahoo.com", "14", "Isabell" },
                    { 12, "Glen_Cummings21@hotmail.com", "13", "Onie" },
                    { 11, "Wyman.Smith@yahoo.com", "12", "Myriam" },
                    { 10, "Adelia.Prosacco@hotmail.com", "11", "Glenna" },
                    { 9, "Gracie_Herzog@hotmail.com", "10", "Danyka" },
                    { 8, "Andy_Reichel@hotmail.com", "9", "Gay" },
                    { 7, "Kamron.Runolfsson@hotmail.com", "8", "Toby" },
                    { 6, "Rickie74@yahoo.com", "7", "Allene" },
                    { 5, "Letha30@yahoo.com", "6", "Kariane" },
                    { 4, "Michael98@hotmail.com", "5", "Crawford" },
                    { 3, "Lincoln_Veum75@yahoo.com", "4", "Lempi" },
                    { 2, "Asia.Conroy@yahoo.com", "3", "Fermin" },
                    { 29, "Kyle.Buckridge53@yahoo.com", "30", "Gunner" },
                    { 30, "Cristobal49@gmail.com", "31", "Salvador" }
                });

            migrationBuilder.InsertData(
                table: "Project",
                columns: new[] { "Id", "Contributors", "Created", "Description", "Name", "ShortDescription", "Updated", "Uri", "UserId" },
                values: new object[,]
                {
                    { 7, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 422, DateTimeKind.Local).AddTicks(6219), "titaitmiuueeeinfbmtsmssmiuieieeuistvemenenxiaoetoeildatnetooisafustidainuaurtleatplamroeaitncfsssbnoacemeqeeeemoridltdtaeoqortlraiqbetclptmlmeagiraieu", "Small Wooden Soap", "mnnencpnetrtiaetoiuenlteuesbtr", new DateTime(2020, 3, 27, 17, 21, 55, 422, DateTimeKind.Local).AddTicks(6266), "http://clark.net", 1 },
                    { 9, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 428, DateTimeKind.Local).AddTicks(5592), "qieiorqiieqmrtaoaittceaeoeotqqtuiettntbepaueslqeixluiuclseuroastqiraeoooiscsueiassttceoaupldocseulaueciutxireoqpimttstlanqiasdrsouistustrrairustemeuqe", "Sleek Cotton Table", "mqmeddslaiieqegmtitlesettvicsr", new DateTime(2020, 3, 27, 17, 21, 55, 428, DateTimeKind.Local).AddTicks(5646), "http://anais.info", 26 },
                    { 4, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 414, DateTimeKind.Local).AddTicks(2027), "taouoeeabeurserlmrvqrosmnqaulladmotuereludunungamesvtiteuiimrluanisltslgtaraldueouneftaumamsqrutsetcmmpqtivupteteuaafudaieuuvbmtaeudsluetdtunelnmaaaso", "Awesome Plastic Hat", "vomtoeiniuoutlosiiediixepunrtr", new DateTime(2020, 3, 27, 17, 21, 55, 414, DateTimeKind.Local).AddTicks(2065), "https://ransom.info", 24 },
                    { 3, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 411, DateTimeKind.Local).AddTicks(4290), "qondauvvvoumlmrceelqiqrdvrumrtsupteldiesetemntuootumxecaneuueosomtaequemdosstoatmirsomtnmusiguenudintnubiooiaaersstunssnrtttiigtqeusiateedainolqeovoic", "Unbranded Rubber Table", "pmetendmmfalmauadndimtnmeeaqqq", new DateTime(2020, 3, 27, 17, 21, 55, 411, DateTimeKind.Local).AddTicks(4336), "http://erling.info", 23 },
                    { 20, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 460, DateTimeKind.Local).AddTicks(739), "tlnasdttulauepqaumetmnispiacdxnetmstiiomsotaqsaieoseoxuauspauaiadsauoamenintmemruneiiuecnrumnieiibltanlueiecdbteimatsqutuorliqurearmiautmeotraiuvneiic", "Licensed Wooden Tuna", "erooitaeoimctiitouiuleesulsfea", new DateTime(2020, 3, 27, 17, 21, 55, 460, DateTimeKind.Local).AddTicks(778), "http://maya.biz", 20 },
                    { 2, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 408, DateTimeKind.Local).AddTicks(5075), "ouadddaosluttstdatheettopioipiimtudsieeuqnauhtsuoaeeammmiamqtotneaenlomtrrnmoomqioopenqmseiainlinufoeemslittiscuatuaeteuauttxviceirutmaauoitqstarieinu", "Incredible Fresh Chips", "diiambnnsriuitgosainaxsioicfad", new DateTime(2020, 3, 27, 17, 21, 55, 408, DateTimeKind.Local).AddTicks(5136), "https://gabriella.biz", 16 },
                    { 26, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 476, DateTimeKind.Local).AddTicks(9514), "ddsnlmbsbtnesurusqtqoomltoeiicdetsatnasrsptassqnarqursneannsiodededieecevusemeutemiatimlqeonselnusmteisuaieeaiamasqduteaeteulubttqicpttnsadpusonuqmaop", "Handcrafted Fresh Hat", "luadtiauceqemqqlaceadeseupemea", new DateTime(2020, 3, 27, 17, 21, 55, 476, DateTimeKind.Local).AddTicks(9553), "http://isabelle.info", 15 },
                    { 25, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 474, DateTimeKind.Local).AddTicks(1587), "oeuuteelslbddodutimcottusieoltdatqqmlotednsxcuqniltoqseiscreitltuotiutouueatoeunanietsluilutievdmiaucmxassotouptotsuuabaoeeeoufarslattleptiemieiittrrv", "Refined Concrete Ball", "ruseteqeudeoxqdieoaleaotesaups", new DateTime(2020, 3, 27, 17, 21, 55, 474, DateTimeKind.Local).AddTicks(1630), "https://john.info", 15 },
                    { 16, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 448, DateTimeKind.Local).AddTicks(8438), "uatosaptuucsplurtepmutstqdtttiootbmtoummsioiamuirntuueuustaluncvppnatemeuhtlstecttiloaiaxmuaspppiianiseurqelgsaatoonterittatourqospsnnudnlenoenmomsnae", "Tasty Plastic Sausages", "itaueaodfitirnueoqmcanquqdeaai", new DateTime(2020, 3, 27, 17, 21, 55, 448, DateTimeKind.Local).AddTicks(8485), "http://victoria.com", 15 },
                    { 10, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 431, DateTimeKind.Local).AddTicks(4349), "audettueeiuauilsadlstmialciimaniomaeusadietndvaheeiucnlnodtrsqiuumrateaaucveioipadttsursnestauiodantcesiupensmltdisarauutettmolltatdoaeueoetnseucctals", "Refined Cotton Ball", "eeuqcoulurnuendleainoisneimneu", new DateTime(2020, 3, 27, 17, 21, 55, 431, DateTimeKind.Local).AddTicks(4398), "https://celia.com", 15 },
                    { 14, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 443, DateTimeKind.Local).AddTicks(448), "tiuermetutuovgvqdstrrmteuosnseqeidtouambaneiolmuuinnmiociesommotufohrlelueuieuiddlatecsoittnosisissvaensopmvareucttuqelltprsoopitlmuasuqseacudanimamum", "Small Frozen Pants", "iluuquvneartiutieaevspeantipsu", new DateTime(2020, 3, 27, 17, 21, 55, 443, DateTimeKind.Local).AddTicks(485), "http://marcelino.info", 14 },
                    { 24, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 471, DateTimeKind.Local).AddTicks(2555), "tutemmrriilaatioliopttciaetutdpuvlnouqqootpmmrtirumpeupuololtetoaetrotuoonudfmtouiosrmdpamiuiitqlniaeomstelalrmiuiucriiinuodaeuiasurniitfmeaunaetsosut", "Ergonomic Concrete Table", "noadiniiaiooanrldqtoueiusgtett", new DateTime(2020, 3, 27, 17, 21, 55, 471, DateTimeKind.Local).AddTicks(2602), "http://garett.org", 13 },
                    { 15, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 445, DateTimeKind.Local).AddTicks(9658), "tmeinmnotmauuaeimoaauaspstrsdcatdmsrrtaeienuemltvsuuqertoeertmtscttqlnaflhseeupqueuaafaresqtusamisuctgiauudqtvmaloailuienmoemaoeaeqieloraoaaaadaseiums", "Generic Soft Table", "tustpirmuaerlotupseraiaeusttti", new DateTime(2020, 3, 27, 17, 21, 55, 445, DateTimeKind.Local).AddTicks(9704), "https://solon.org", 12 },
                    { 8, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 425, DateTimeKind.Local).AddTicks(6486), "odesumnefgeeertistiariereqddueslpnineeraerrttrrvcimdremairlransplaouatnostidprcsustdiuuhtuantpirpssaotqdsleadqnuoremalldviiruuoqeiuumrdlsartndaamoiiau", "Rustic Rubber Mouse", "ceaelinlatuutnluertamsqcsoruit", new DateTime(2020, 3, 27, 17, 21, 55, 425, DateTimeKind.Local).AddTicks(6540), "http://raymond.name", 12 },
                    { 22, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 465, DateTimeKind.Local).AddTicks(6597), "dqlroaaplauueicfuittimamrltembsieilroaveteonentssotunmppiuiineluemtannoudbmeuiusnmittueuimiendgrfevemrduiuspleeelcsisqaerhqoesirtetanliteostmnutioeaio", "Handmade Soft Chair", "mteeqnotemdmatrrtarqtuulliseaq", new DateTime(2020, 3, 27, 17, 21, 55, 465, DateTimeKind.Local).AddTicks(6653), "http://herminio.org", 11 },
                    { 1, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 402, DateTimeKind.Local).AddTicks(7546), "pteoiuaaeoauuuioadiueelmuqeecxtdptenoootreelstuuqioerqtsnateqoeieaxrduqmsaieoeulmlitmsnnatsmoeusceaniueseeadaudnepiqseadoqpiaumipoenoeaedttinusfsmprls", "Unbranded Plastic Fish", "nsqqsnitarllnmosnecmdqoeddmqaa", new DateTime(2020, 3, 27, 17, 21, 55, 405, DateTimeKind.Local).AddTicks(6452), "http://guiseppe.biz", 11 },
                    { 29, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 485, DateTimeKind.Local).AddTicks(3224), "cvsmotsioimtioutstmuqmaiqftaxeuustteeluudrtdoelmuiirvaelgteaiisliratmuietiouiedvnibuosanaqluqmhtuovexeettrlnlamstxaatcduoutuddbotumtuaeittluoieusasati", "Awesome Concrete Pants", "rheersteqeutaauouuormtutearaqi", new DateTime(2020, 3, 27, 17, 21, 55, 485, DateTimeKind.Local).AddTicks(3270), "http://wendy.name", 10 },
                    { 17, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 451, DateTimeKind.Local).AddTicks(6497), "ereuppsnucuuirtiqtoeiiarrtousuiucsaaaqenoeqiaesqoaavpsaiepiunanmisuoumtsuunuieoeaueitauruooitaincteiuedrueouslummisioiieooeteetuguiostoieeriguiemtbesl", "Fantastic Plastic Ball", "tsteualtitiltemrnemiutdppeqeoe", new DateTime(2020, 3, 27, 17, 21, 55, 451, DateTimeKind.Local).AddTicks(6531), "https://gilberto.org", 10 },
                    { 21, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 462, DateTimeKind.Local).AddTicks(6628), "sdsosumoeieimosascaegumtcneaitaeforasctrinvtiatstsoalavimousuinlatsurnnuistuepaldutitluatattiassimiivdoeqstarofmtrdsitptstimediqistleelocieisiuereucee", "Ergonomic Frozen Shoes", "tsteuntlbouitsqudeemeupeniamue", new DateTime(2020, 3, 27, 17, 21, 55, 462, DateTimeKind.Local).AddTicks(6678), "http://john.name", 9 },
                    { 13, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 440, DateTimeKind.Local).AddTicks(1939), "embixudqiueitalsmlntreinimtimnstienpuacuisqdsitcteeplsnisterttscvudnimuiersxtnaeranpsxtnmettcaqnuieaiblbtesddlcmtseaauquefuatsfmutooeottbimloeucnsvevo", "Rustic Cotton Mouse", "ttctvipderaatsrumesipeisiuivlq", new DateTime(2020, 3, 27, 17, 21, 55, 440, DateTimeKind.Local).AddTicks(1988), "http://velda.org", 9 },
                    { 27, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 479, DateTimeKind.Local).AddTicks(7144), "sctemaattetusqanseteieoatraaileruimutuasstouetisteousioeeesmauocepopuervuutuiposaodaitntodiunstmioemroautainuqtuetivalduisaenueqtsqelohstpeteieiuvbuic", "Unbranded Steel Pants", "eielontottsiresiauutotsnteufmm", new DateTime(2020, 3, 27, 17, 21, 55, 479, DateTimeKind.Local).AddTicks(7197), "https://skyla.info", 8 },
                    { 18, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 454, DateTimeKind.Local).AddTicks(5340), "eaaooetsrtiaiemietooeaurltpeuunemmnmsaiaaroplrrmialsrtaersslrotuunnvmamlttatuvrnehetieceibtapiuaosuchspoeeitapfuruaaagoaaeeeoenleeiteureuauuileasiuumi", "Tasty Frozen Fish", "vguueeesppseatadcudattpsreadan", new DateTime(2020, 3, 27, 17, 21, 55, 454, DateTimeKind.Local).AddTicks(5383), "http://jettie.name", 7 },
                    { 12, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 437, DateTimeKind.Local).AddTicks(2718), "uddsouselaromtuaptoetepaleaddslanqgtttotaumetrienltruulusumuemetnedeooivinuittotoectdomoeuoeouasiitsouiaeaiooasiotieqnqupctoutiutlqpuoceuutuuposeoansc", "Refined Soft Computer", "iaqtavuelimfsmsapteobaqeuratue", new DateTime(2020, 3, 27, 17, 21, 55, 437, DateTimeKind.Local).AddTicks(2763), "https://fabian.org", 6 },
                    { 11, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 434, DateTimeKind.Local).AddTicks(2892), "seadmaedrailausuisudieepqiaouitutnicdeaasdlsuucfacutturiuheqmmrlaaaseeodtnuusoaemeisuuenfenaubtnhixcdseqraceieaaiieeuesupmteritaqnmoeseotstitetnutedmo", "Rustic Fresh Bike", "ttmitshlemaseraenamentuomqpgre", new DateTime(2020, 3, 27, 17, 21, 55, 434, DateTimeKind.Local).AddTicks(2940), "http://johathan.biz", 6 },
                    { 6, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 419, DateTimeKind.Local).AddTicks(7062), "itiiiesqiuurtcoqtienremssutaoiudrndmletsneatttneqsueemttnnteettqsrireadmgondrrssvpateltlfiatmuiitudiqimeamundmrieitostaembutalartitenetllntamspiouedil", "Rustic Plastic Pants", "sssidsmvtdiuseoiifnppeofaeuptn", new DateTime(2020, 3, 27, 17, 21, 55, 419, DateTimeKind.Local).AddTicks(7104), "https://bryana.org", 6 },
                    { 5, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 416, DateTimeKind.Local).AddTicks(9439), "eseqcmtioeoilirtttasiturnleouuivonnitllnatitttdteartiuiotstaaaderiaaqetlilmcimtemiloeilclaiuotouxncsinemiiaibsptnolpdiriqgpudrusmepessiiniqumutetamrae", "Generic Concrete Tuna", "seqdvaaoitrdaampsasioinludeenb", new DateTime(2020, 3, 27, 17, 21, 55, 416, DateTimeKind.Local).AddTicks(9476), "http://dario.org", 6 },
                    { 28, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 482, DateTimeKind.Local).AddTicks(5882), "ahxoeeuloetuhbinmimteralnpecatriqotatiadtaosdiaouonarestimqeuesamlfsimetiieisactnsdtqeroanilnsqquoonltutivsresttetoauasmassdaieigtsanlnmmaurtlmtuelrst", "Refined Cotton Mouse", "uoiulteuirtoreuiqteqriintqqqoa", new DateTime(2020, 3, 27, 17, 21, 55, 482, DateTimeKind.Local).AddTicks(5940), "http://diego.org", 4 },
                    { 19, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 457, DateTimeKind.Local).AddTicks(2996), "osqqaevdagrisubiihlusivoesetpolutaeutiocnusetoeeiiniercsdvleoaamtsleemtbuenctipluouludeiiirssltueteugmaqensidqtdtttmserdaasuuetcxeeiictiemvhrnvqmireiu", "Licensed Rubber Shirt", "laatorsnupauiualuatlvehtednnqv", new DateTime(2020, 3, 27, 17, 21, 55, 457, DateTimeKind.Local).AddTicks(3033), "https://lenna.net", 4 },
                    { 23, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 468, DateTimeKind.Local).AddTicks(5732), "nmietaoadqasoeitttsaqteoadceteetsacreoieimodiernuruasrxueqedltuitdmxouleotsndulnsgnoecuustqtrneltelaolgqrioloqioenmlamnsopimpttldstberresnaattuuetutqa", "Ergonomic Cotton Bike", "tauutliqluctrdpdaasuueipnueeio", new DateTime(2020, 3, 27, 17, 21, 55, 468, DateTimeKind.Local).AddTicks(5784), "http://vincent.name", 29 },
                    { 30, "[]", new DateTime(2020, 3, 25, 17, 21, 55, 488, DateTimeKind.Local).AddTicks(3435), "uoeluqrsuutmrumisdseuouoeateeoueuausuaenqltidbnisaexvmiurtasuiiauaarsundbpsteesmtbtaeiuunudeiidadaiunsdeemmaegieemvmeeqssuuepehiqlialoucutdolseeetsotn", "Practical Granite Pants", "stptaptpoteiaieifrttcrebnpopns", new DateTime(2020, 3, 27, 17, 21, 55, 488, DateTimeKind.Local).AddTicks(3488), "http://eli.info", 29 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Project_UserId",
                table: "Project",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
