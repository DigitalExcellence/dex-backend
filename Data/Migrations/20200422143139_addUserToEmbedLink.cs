using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class addUserToEmbedLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "EmbeddedProject",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "IdentityId", "Name", "ProfileUrl" },
                values: new object[,]
                {
                    { 1, "Nat.Bailey83@hotmail.com", "2", "Nestor", null },
                    { 28, "Hunter_Heller@hotmail.com", "29", "Lilliana", null },
                    { 27, "Nicholas61@hotmail.com", "28", "Pattie", null },
                    { 26, "Joan62@gmail.com", "27", "Pearl", null },
                    { 25, "Aliyah_Ferry5@gmail.com", "26", "Alda", null },
                    { 24, "Philip_Sawayn@yahoo.com", "25", "Destany", null },
                    { 23, "Susan.Hartmann@yahoo.com", "24", "Vicente", null },
                    { 22, "Prudence_Abernathy38@yahoo.com", "23", "Juston", null },
                    { 21, "Ellen.Weimann@hotmail.com", "22", "Elmer", null },
                    { 20, "Gina_Ernser@hotmail.com", "21", "Rosa", null },
                    { 19, "Clarissa96@yahoo.com", "20", "Liliana", null },
                    { 18, "Abdul.Graham@gmail.com", "19", "Kelsie", null },
                    { 17, "Cathy.Wiza49@gmail.com", "18", "Zella", null },
                    { 16, "Genoveva54@hotmail.com", "17", "Buddy", null },
                    { 15, "Kareem.Feeney98@gmail.com", "16", "Rickie", null },
                    { 14, "Florine.Runte89@hotmail.com", "15", "Brianne", null },
                    { 13, "Kallie_Kihn@yahoo.com", "14", "Evelyn", null },
                    { 12, "Clare8@hotmail.com", "13", "Terrence", null },
                    { 11, "Madelyn.Flatley48@gmail.com", "12", "Chet", null },
                    { 10, "Houston_Mraz@gmail.com", "11", "Renee", null },
                    { 9, "Pamela_OConnell@yahoo.com", "10", "Zoie", null },
                    { 8, "Elenor.Emmerich95@gmail.com", "9", "Makayla", null },
                    { 7, "Leon45@gmail.com", "8", "Emmanuel", null },
                    { 6, "Karianne.Koelpin30@gmail.com", "7", "Angelina", null },
                    { 5, "Sidney76@gmail.com", "6", "Lue", null },
                    { 4, "Gabrielle76@gmail.com", "5", "Stephany", null },
                    { 3, "Easton.Grimes@yahoo.com", "4", "Marilyne", null },
                    { 2, "Jessica.Stamm29@yahoo.com", "3", "Johanna", null },
                    { 29, "Vida50@hotmail.com", "30", "Joyce", null },
                    { 30, "Xander49@hotmail.com", "31", "Wayne", null }
                });

            migrationBuilder.InsertData(
                table: "Project",
                columns: new[] { "Id", "Created", "Description", "Name", "ShortDescription", "Updated", "Uri", "UserId" },
                values: new object[,]
                {
                    { 5, new DateTime(2020, 4, 20, 16, 31, 38, 405, DateTimeKind.Local).AddTicks(7294), @"Quo delectus laudantium ut asperiores nulla.
                Et dolore in ut aspernatur laboriosam consectetur.
                Labore voluptatem perferendis corrupti odio voluptatem ut qui accusantium.
                Aut qui sunt minus modi.
                Reiciendis et voluptas aliquid veniam error commodi consequatur.
                Assumenda impedit eum voluptas sint quos hic voluptas accusantium voluptas.
                Enim velit necessitatibus numquam deleniti.
                Odio libero consectetur voluptatem occaecati magni voluptatem accusamus nostrum.
                Sunt quo ut cum distinctio quia quas.
                Iste pariatur illum odit aut at aliquam et.", "Incredible Metal Gloves", "Debitis qui dolor est unde accusamus.", new DateTime(2020, 4, 22, 16, 31, 38, 405, DateTimeKind.Local).AddTicks(7376), "https://kennith.info", 2 },
                    { 7, new DateTime(2020, 4, 20, 16, 31, 38, 425, DateTimeKind.Local).AddTicks(1468), @"Perferendis consequuntur et dolor expedita.
                Assumenda sit deserunt quia aut aut.
                Debitis nihil rem molestiae necessitatibus magnam quibusdam cumque ullam numquam.
                Est ut culpa numquam fugiat ipsum iste vero enim.
                Recusandae minus repellat eligendi nemo quos.
                Labore aut non fuga qui aut.
                Debitis id cumque labore accusantium.
                Distinctio porro voluptates qui ea nemo rerum.
                Aliquid nihil est dolore sed praesentium earum.
                Rem ut culpa omnis vel laudantium quis quis.", "Rustic Plastic Shirt", "Hic perspiciatis voluptatem suscipit.", new DateTime(2020, 4, 22, 16, 31, 38, 425, DateTimeKind.Local).AddTicks(1549), "http://ara.name", 27 },
                    { 6, new DateTime(2020, 4, 20, 16, 31, 38, 415, DateTimeKind.Local).AddTicks(7628), @"Ea blanditiis et saepe autem corrupti atque.
                Est odit eligendi eius distinctio et consequatur sit.
                Suscipit consequatur qui quisquam ratione voluptatum molestias asperiores accusantium.
                Vitae unde delectus officia repellendus nihil reiciendis quos ea deleniti.
                Deleniti voluptas culpa possimus amet quis sunt iure necessitatibus labore.
                Quibusdam eum veniam quo quibusdam at.
                Voluptatibus sed ut aliquid.
                Ea aut quisquam occaecati et quibusdam soluta.
                Est pariatur hic vero.
                Accusamus delectus a amet.", "Generic Metal Bike", "Id dolores qui et facere eos.", new DateTime(2020, 4, 22, 16, 31, 38, 415, DateTimeKind.Local).AddTicks(7697), "http://brandon.net", 26 },
                    { 30, new DateTime(2020, 4, 20, 16, 31, 38, 597, DateTimeKind.Local).AddTicks(3309), @"Ut et soluta qui neque iusto quo quam quam reprehenderit.
                Neque aliquam quasi voluptatem laborum.
                Corrupti voluptates placeat quas a temporibus doloribus iure dolorem.
                Reiciendis laudantium non vel quasi debitis expedita.
                Enim laborum perspiciatis at incidunt totam alias et.
                Culpa quibusdam fugit voluptas nemo facilis asperiores voluptatibus officia.
                Et earum minima eligendi et laboriosam et eos a saepe.
                Voluptatem veniam animi autem debitis alias et.
                Cum veniam quas porro ut voluptate officia quo.
                Libero illum provident fugiat atque alias tempora placeat.", "Practical Granite Tuna", "Et aut sint commodi est quae aliquid et voluptatem excepturi.", new DateTime(2020, 4, 22, 16, 31, 38, 597, DateTimeKind.Local).AddTicks(3370), "http://destini.net", 25 },
                    { 8, new DateTime(2020, 4, 20, 16, 31, 38, 430, DateTimeKind.Local).AddTicks(2907), @"Qui perspiciatis quis repellat.
                Eos rerum qui minima vel quis ut.
                Amet magni totam expedita ut.
                Velit molestiae dolorum quis.
                Voluptas similique eius quia tenetur sit.
                Nulla harum est officiis non hic.
                Voluptatem occaecati in nihil aut repellendus nam.
                Repellat voluptatem explicabo alias.
                Error corrupti et temporibus velit quaerat fugiat incidunt.
                Rerum voluptas et.", "Refined Metal Bacon", "Delectus odio porro mollitia ipsam sed sint architecto fugit soluta.", new DateTime(2020, 4, 22, 16, 31, 38, 430, DateTimeKind.Local).AddTicks(2960), "http://jane.biz", 24 },
                    { 14, new DateTime(2020, 4, 20, 16, 31, 38, 463, DateTimeKind.Local).AddTicks(7871), @"Minima qui placeat adipisci perferendis fuga sit corrupti ipsum quia.
                Et sit nobis doloribus deserunt.
                Nostrum ratione hic.
                Officiis aut qui.
                Deserunt repudiandae eum fugiat explicabo dolor sit a.
                Et officia et et et.
                Corporis quaerat praesentium.
                Et eos non natus.
                Ut velit sint dicta laudantium debitis voluptate dolores dolorem.
                Quam impedit et et beatae rerum amet.", "Licensed Concrete Shirt", "Ab dolorum in tempore et tempore ipsa facilis temporibus velit.", new DateTime(2020, 4, 22, 16, 31, 38, 463, DateTimeKind.Local).AddTicks(7949), "https://brenna.biz", 23 },
                    { 25, new DateTime(2020, 4, 20, 16, 31, 38, 563, DateTimeKind.Local).AddTicks(7719), @"Voluptatem ratione alias autem ea quod nulla consequatur.
                Et quia eos nesciunt dolor tempore exercitationem non.
                Non natus rerum rerum.
                Velit iusto qui.
                Cumque est quia possimus nisi quas vel voluptas maiores porro.
                Et ea facere dolores accusantium suscipit qui et.
                Qui illum aut dolores ut neque qui est.
                Doloremque et itaque et dolorem.
                Mollitia blanditiis officiis fugiat.
                Nisi laboriosam est in dolores eum non consequatur.", "Handmade Wooden Ball", "Et dolor aut qui nihil occaecati aut omnis ipsa.", new DateTime(2020, 4, 22, 16, 31, 38, 563, DateTimeKind.Local).AddTicks(7775), "http://elmore.info", 22 },
                    { 17, new DateTime(2020, 4, 20, 16, 31, 38, 478, DateTimeKind.Local).AddTicks(9608), @"Fugit dolorum sunt quaerat ut ea nostrum excepturi sunt facilis.
                Possimus quis unde laboriosam fuga corrupti dolorem.
                Ab accusamus eaque quia quo earum.
                Id magni eum tenetur nobis ullam.
                Sint repellendus veniam corporis et et architecto quaerat.
                Vero molestias nihil dolorem ea.
                Iure et ducimus quasi facere.
                Odit architecto ullam.
                Ipsam rerum dolorum neque consequuntur et.
                Dolore vitae est omnis illo quia eos et quam autem.", "Unbranded Concrete Car", "Sit vel consequatur fugit aut voluptate quod.", new DateTime(2020, 4, 22, 16, 31, 38, 478, DateTimeKind.Local).AddTicks(9673), "https://justice.org", 22 },
                    { 26, new DateTime(2020, 4, 20, 16, 31, 38, 568, DateTimeKind.Local).AddTicks(509), @"Dolor saepe culpa est sed sunt laudantium error voluptatibus ea.
                Magni iusto rerum.
                Harum sint debitis qui.
                Repellat fugit sed corporis.
                Fugiat esse et et eos.
                Vel minus distinctio quo.
                Hic sed ad ut voluptatem et.
                Ullam laudantium facere voluptas in veritatis unde at dolore.
                Nihil sint nam est.
                Nisi nihil sequi consequatur voluptates.", "Rustic Granite Sausages", "Sit et in.", new DateTime(2020, 4, 22, 16, 31, 38, 568, DateTimeKind.Local).AddTicks(591), "http://chester.net", 19 },
                    { 18, new DateTime(2020, 4, 20, 16, 31, 38, 488, DateTimeKind.Local).AddTicks(586), @"Maiores molestiae enim sit qui explicabo dolores labore.
                Blanditiis maxime qui similique sit qui officia.
                Suscipit ut quas saepe.
                Quis velit repellendus numquam quam est.
                Nisi cumque iusto.
                Illo quasi qui unde ut.
                Cumque totam rerum ut omnis quis sint voluptas velit saepe.
                Laudantium numquam velit et illum et iste aut ut labore.
                Corrupti consequuntur sed possimus nemo eius excepturi quaerat.
                Eaque magnam consequatur sed.", "Awesome Metal Shirt", "Hic numquam illum.", new DateTime(2020, 4, 22, 16, 31, 38, 488, DateTimeKind.Local).AddTicks(653), "http://jodie.name", 17 },
                    { 15, new DateTime(2020, 4, 20, 16, 31, 38, 471, DateTimeKind.Local).AddTicks(453), @"Eveniet distinctio aspernatur dolores ea.
                Blanditiis facere ut velit qui aut est eum.
                Molestiae adipisci nesciunt earum quam.
                Tenetur nulla ipsa illum sunt asperiores commodi velit architecto.
                Ea corrupti ea.
                Veniam sunt qui est.
                Earum sint aut voluptatum est voluptas sit accusantium voluptatum sed.
                Tempora velit deserunt molestiae earum.
                Illo cum ipsam nam explicabo iste nesciunt nesciunt aperiam.
                Dolore quia perferendis laboriosam maxime.", "Fantastic Plastic Chair", "Reiciendis voluptatum rerum rerum quis reprehenderit reiciendis.", new DateTime(2020, 4, 22, 16, 31, 38, 471, DateTimeKind.Local).AddTicks(512), "https://vita.info", 17 },
                    { 11, new DateTime(2020, 4, 20, 16, 31, 38, 445, DateTimeKind.Local).AddTicks(895), @"Mollitia consequuntur delectus aperiam.
                Architecto alias sed dicta explicabo fugit quia tenetur commodi saepe.
                Sed et rerum doloribus qui quae id eaque neque.
                Provident voluptas et.
                Qui placeat et nesciunt et minima et officia.
                Quibusdam perferendis magni non cum et.
                Recusandae fugit repellendus voluptas nesciunt.
                Ut animi ut unde iure molestiae officiis et ut neque.
                Autem excepturi commodi sapiente autem praesentium et.
                Laborum ipsum sunt reiciendis blanditiis quo veritatis velit.", "Generic Concrete Soap", "Sunt esse illo omnis omnis id in rerum dolorum.", new DateTime(2020, 4, 22, 16, 31, 38, 445, DateTimeKind.Local).AddTicks(952), "https://lucius.info", 17 },
                    { 20, new DateTime(2020, 4, 20, 16, 31, 38, 506, DateTimeKind.Local).AddTicks(7421), @"Iusto maiores in est ipsa aperiam iusto temporibus est harum.
                Iure et nihil accusantium.
                Error ut beatae saepe distinctio consequatur ratione.
                Fugit eum placeat.
                Numquam sit atque porro numquam unde.
                Voluptas ea repellat sunt fuga adipisci suscipit.
                Ad dolore vero illum dolor omnis.
                Ut maiores suscipit id ipsam quasi quo.
                In numquam quis sit in.
                Ratione odio quia vitae ratione amet sequi similique eligendi.", "Gorgeous Cotton Shoes", "Cumque quia aut officiis consequatur aliquam.", new DateTime(2020, 4, 22, 16, 31, 38, 506, DateTimeKind.Local).AddTicks(7499), "https://caden.net", 16 },
                    { 3, new DateTime(2020, 4, 20, 16, 31, 38, 389, DateTimeKind.Local).AddTicks(3553), @"Magnam porro illum sit dolor officia qui.
                Soluta alias est tempore.
                Dolorum hic at.
                Nulla voluptatibus ut.
                Rerum similique doloremque id.
                Ducimus iste fugiat impedit praesentium earum officia ut ipsa.
                Ullam impedit et perferendis libero ad consequatur.
                Pariatur beatae necessitatibus necessitatibus repudiandae.
                Consequatur enim illo illum quam corporis ratione.
                Deserunt voluptates qui eveniet nisi aut ut nihil vitae.", "Ergonomic Steel Pants", "In suscipit iste sit velit delectus officiis officiis.", new DateTime(2020, 4, 22, 16, 31, 38, 389, DateTimeKind.Local).AddTicks(3629), "https://hellen.com", 16 },
                    { 12, new DateTime(2020, 4, 20, 16, 31, 38, 448, DateTimeKind.Local).AddTicks(5513), @"Perspiciatis fugiat neque voluptatem reprehenderit eos expedita ipsum aut.
                Consequatur quaerat fuga vitae quo voluptas sint.
                Vel quia aut.
                Qui voluptates delectus voluptatem quia odit est mollitia eveniet.
                Fugiat magni repudiandae alias eveniet ratione velit tenetur.
                Rerum tempore aut autem qui sit sed rerum natus.
                Facilis est dolores quod minus aut aliquam.
                Architecto sapiente unde dignissimos quas.
                Sit molestiae dolores ipsum quo molestiae consequatur expedita.
                Ea tempora dolores vel sed dolorem corporis rerum totam.", "Handmade Granite Ball", "Aut magnam ut optio.", new DateTime(2020, 4, 22, 16, 31, 38, 448, DateTimeKind.Local).AddTicks(5563), "http://sibyl.com", 14 },
                    { 10, new DateTime(2020, 4, 20, 16, 31, 38, 439, DateTimeKind.Local).AddTicks(3547), @"Velit quo ut porro.
                Iste et voluptas doloremque ea quos minus aliquam voluptas.
                Et sit iusto ipsa est eum non a sed quibusdam.
                Quae quo id quia nisi aut.
                Amet qui asperiores mollitia quae itaque.
                Maxime possimus quo dicta occaecati laudantium nesciunt ad.
                Ipsa labore eos.
                Eum dolorem doloribus possimus consequatur.
                Nisi excepturi omnis quisquam voluptates perferendis sint quaerat quasi dolor.
                Fugiat veniam et ea doloremque fuga autem accusamus perferendis suscipit.", "Incredible Plastic Soap", "Est quod natus rerum deleniti.", new DateTime(2020, 4, 22, 16, 31, 38, 439, DateTimeKind.Local).AddTicks(3605), "http://jaquan.name", 14 },
                    { 21, new DateTime(2020, 4, 20, 16, 31, 38, 515, DateTimeKind.Local).AddTicks(5754), @"Laborum ut corrupti alias et quam.
                Laboriosam quia omnis rerum consequuntur.
                Soluta id aperiam in molestiae libero nostrum aut et.
                Explicabo delectus provident voluptatem sed porro commodi rerum ut facilis.
                Natus nulla expedita.
                Sunt ratione sit fugit.
                Error sit placeat repellat sunt ad sed.
                Sed fugit non.
                Impedit dolore reprehenderit eaque sed aut.
                Fuga vel illum tempore.", "Generic Soft Hat", "Placeat quia necessitatibus autem reprehenderit laboriosam.", new DateTime(2020, 4, 22, 16, 31, 38, 515, DateTimeKind.Local).AddTicks(5805), "http://keon.name", 8 },
                    { 2, new DateTime(2020, 4, 20, 16, 31, 38, 381, DateTimeKind.Local).AddTicks(4144), @"Odio beatae ipsam voluptas eius culpa dicta ut dolorem et.
                Ut minus expedita deleniti consequatur tempore et vero voluptas vel.
                Est debitis placeat mollitia sapiente beatae maiores.
                Laudantium eos nulla voluptatem asperiores rerum non voluptas quia consequatur.
                Nihil ratione reprehenderit consequuntur ipsum dolorum sit neque eligendi unde.
                Illum harum perspiciatis officiis et.
                Culpa fuga et consequatur repudiandae veniam.
                Vitae inventore qui distinctio mollitia odio fugiat non corrupti at.
                Quam aliquam facere in reiciendis rerum repudiandae exercitationem suscipit fugit.
                Non id quia.", "Tasty Metal Shoes", "Quos voluptatem et placeat voluptatibus molestias.", new DateTime(2020, 4, 22, 16, 31, 38, 381, DateTimeKind.Local).AddTicks(4239), "https://angelina.biz", 8 },
                    { 29, new DateTime(2020, 4, 20, 16, 31, 38, 590, DateTimeKind.Local).AddTicks(8578), @"Sit rerum quisquam ut esse illo.
                Rerum dolore magnam rerum amet praesentium.
                Velit officiis non.
                Qui in natus qui rerum illum.
                Modi in hic neque recusandae ut quasi soluta.
                Cupiditate quasi qui dolorem ea sint enim qui qui.
                Ut nam at nam quos et voluptatem quas.
                Illo molestiae fuga alias et ut adipisci.
                Iusto quisquam enim minima similique quas.
                Deserunt laboriosam nostrum commodi sit libero corporis.", "Refined Wooden Tuna", "Nam non repudiandae a consequatur voluptates.", new DateTime(2020, 4, 22, 16, 31, 38, 590, DateTimeKind.Local).AddTicks(8642), "https://orrin.net", 7 },
                    { 27, new DateTime(2020, 4, 20, 16, 31, 38, 578, DateTimeKind.Local).AddTicks(4666), @"Non possimus minima.
                Quo excepturi quas perferendis deserunt odit consequatur aut est accusantium.
                Ab et tempore vero sunt consectetur excepturi sit.
                Fugit modi ex omnis voluptatum in et eos.
                Beatae omnis nesciunt iure dolor odio fugiat velit.
                Numquam aut sed quia.
                Quis doloremque officiis eos aut vero aperiam.
                Quam molestiae natus autem et nesciunt tenetur ut adipisci.
                Et suscipit est magnam.
                Nam voluptatem ut accusantium quis ab exercitationem.", "Handcrafted Metal Soap", "Mollitia et nam voluptatibus cupiditate animi sed.", new DateTime(2020, 4, 22, 16, 31, 38, 578, DateTimeKind.Local).AddTicks(4731), "https://thurman.name", 7 },
                    { 24, new DateTime(2020, 4, 20, 16, 31, 38, 556, DateTimeKind.Local).AddTicks(8695), @"Hic debitis et aliquid.
                In quia perspiciatis id et non.
                Ex quos distinctio officiis vel harum hic vel et dolor.
                Molestias vero nostrum quia et et ipsum ducimus.
                Accusamus modi est.
                Porro ut aperiam ducimus sequi odit est.
                Id veniam et perferendis corporis nisi beatae nesciunt ducimus id.
                Assumenda consectetur ex.
                Consectetur beatae cupiditate voluptatum iusto autem.
                Odit magni quisquam sunt nesciunt ratione.", "Licensed Cotton Table", "Et expedita sapiente sunt facere et in.", new DateTime(2020, 4, 22, 16, 31, 38, 556, DateTimeKind.Local).AddTicks(9218), "https://curtis.info", 7 },
                    { 22, new DateTime(2020, 4, 20, 16, 31, 38, 524, DateTimeKind.Local).AddTicks(4635), @"Provident maxime non sit.
                Quod at iusto error at.
                Ut qui iste ipsa dolor quos quo.
                Distinctio enim magnam qui eum neque est eos.
                Magnam est voluptas illum et qui alias.
                Modi odio provident architecto illum.
                Sit ut nostrum.
                Dolore dolor voluptatum.
                Minima libero soluta odit rerum minima nostrum unde sit.
                Debitis quaerat amet voluptas cupiditate dolor rerum.", "Fantastic Cotton Cheese", "Omnis nobis a ab sit.", new DateTime(2020, 4, 22, 16, 31, 38, 524, DateTimeKind.Local).AddTicks(4719), "https://verona.net", 6 },
                    { 16, new DateTime(2020, 4, 20, 16, 31, 38, 475, DateTimeKind.Local).AddTicks(1727), @"Saepe non labore laudantium aperiam est suscipit ullam pariatur commodi.
                Ipsam laborum non recusandae qui praesentium velit omnis porro.
                Sed atque repellat ducimus nam cumque.
                Optio sed saepe vel ut sunt.
                Voluptatem delectus unde nemo ut ipsum.
                Fuga qui explicabo illo dolor qui aut ratione.
                Laudantium tempora explicabo excepturi omnis dignissimos.
                Sit quis architecto non.
                Est totam rem delectus cupiditate aut laborum.
                Repudiandae necessitatibus culpa perspiciatis facere quos eveniet ut reprehenderit.", "Handcrafted Soft Bacon", "Magnam iusto unde omnis molestiae voluptatem aliquam culpa facilis officiis.", new DateTime(2020, 4, 22, 16, 31, 38, 475, DateTimeKind.Local).AddTicks(1778), "https://viviane.org", 5 },
                    { 13, new DateTime(2020, 4, 20, 16, 31, 38, 454, DateTimeKind.Local).AddTicks(2188), @"Numquam odio dolorem commodi laborum quo occaecati commodi non magni.
                Doloribus et omnis excepturi animi.
                Eos rem assumenda voluptatem.
                Minima non qui.
                Qui odit asperiores quisquam repellendus.
                Voluptatem accusamus ratione repellendus delectus qui quas.
                Dolor ad ut.
                Quisquam voluptatem eaque.
                Debitis culpa autem molestias autem similique tempore dolores neque quidem.
                Suscipit maiores velit quis quia ut minus libero asperiores harum.", "Generic Fresh Pants", "Harum maxime harum commodi maiores ipsa ut dolore.", new DateTime(2020, 4, 22, 16, 31, 38, 454, DateTimeKind.Local).AddTicks(2260), "http://chauncey.org", 5 },
                    { 9, new DateTime(2020, 4, 20, 16, 31, 38, 433, DateTimeKind.Local).AddTicks(4553), @"Quasi accusantium tenetur libero pariatur ex alias.
                Tempore tempore sed aliquid fugit aut vel architecto veritatis qui.
                Deleniti in quo saepe.
                Officia ducimus consequatur consequatur et officia.
                Ipsa distinctio totam sunt id cum minima fugit dicta.
                Nam rerum provident in quisquam alias et consequatur.
                Unde soluta similique explicabo ipsum ut dolores totam.
                Ea perspiciatis asperiores.
                Dicta repudiandae architecto est facilis enim architecto qui repellendus.
                Dolores voluptas minus adipisci ea vitae accusamus ducimus.", "Sleek Soft Towels", "Velit velit eaque.", new DateTime(2020, 4, 22, 16, 31, 38, 433, DateTimeKind.Local).AddTicks(4600), "https://mohamed.name", 4 },
                    { 1, new DateTime(2020, 4, 20, 16, 31, 38, 365, DateTimeKind.Local).AddTicks(1487), @"Sit quo quo possimus sint quas.
                Excepturi dolorem aut minima officia.
                Perspiciatis ullam dolorem omnis sunt tempore veritatis asperiores.
                Reprehenderit quidem quia omnis voluptatem voluptatem.
                Eius consequatur hic enim.
                Voluptatum modi consequatur sunt dignissimos laborum voluptas asperiores aut sint.
                Aut rem rerum dolorem hic quidem est ab.
                Praesentium sed quia.
                Iusto delectus et.
                Nisi hic architecto voluptas.", "Licensed Metal Keyboard", "Dolorem et architecto ex tempore.", new DateTime(2020, 4, 22, 16, 31, 38, 374, DateTimeKind.Local).AddTicks(6754), "http://bette.info", 4 },
                    { 4, new DateTime(2020, 4, 20, 16, 31, 38, 395, DateTimeKind.Local).AddTicks(6131), @"Ut nihil consectetur necessitatibus est molestiae quod totam.
                Sed cupiditate officiis assumenda non quo quisquam.
                Asperiores ratione ipsam culpa.
                Cum vero dicta.
                Unde dolorem quo est recusandae non qui aliquam beatae enim.
                Illum enim illum quo et.
                Non necessitatibus explicabo.
                Earum ullam deserunt.
                Nam eum et.
                Magnam doloremque sit.", "Handcrafted Frozen Towels", "Dolorem autem sunt et ut amet deserunt nostrum sint.", new DateTime(2020, 4, 22, 16, 31, 38, 395, DateTimeKind.Local).AddTicks(6205), "http://elta.biz", 3 },
                    { 28, new DateTime(2020, 4, 20, 16, 31, 38, 582, DateTimeKind.Local).AddTicks(6376), @"Doloribus qui tempore dolore animi voluptatem error magnam officiis.
                Quia dignissimos non voluptas est eum distinctio aut quia ut.
                Rerum hic quia et itaque expedita iure qui est iure.
                At quo doloremque esse aliquid consequuntur ducimus enim a ut.
                Porro eos beatae.
                Optio vitae ducimus hic esse ut.
                Eos odit officiis et vel nam consequatur dolor nam.
                Natus incidunt officia quis.
                Ea deleniti provident ea at earum et at incidunt.
                Aliquid perspiciatis a corporis dolores.", "Intelligent Plastic Fish", "Aut et accusantium reiciendis molestiae perspiciatis at enim repellendus.", new DateTime(2020, 4, 22, 16, 31, 38, 582, DateTimeKind.Local).AddTicks(6467), "https://dylan.name", 2 },
                    { 19, new DateTime(2020, 4, 20, 16, 31, 38, 495, DateTimeKind.Local).AddTicks(4133), @"Atque accusantium praesentium labore similique voluptas odio nam est accusantium.
                Illum veritatis quibusdam et exercitationem sit voluptatem impedit debitis numquam.
                Officiis accusamus accusamus veniam id veniam delectus accusamus distinctio.
                Recusandae quia enim minima omnis ad est ut.
                Qui facilis excepturi ab magni sequi voluptatem.
                Aut ipsa vero quae voluptatem nam rerum sequi deserunt.
                Quis aut sequi cumque vero.
                Magnam aut id cupiditate repellendus.
                Illo praesentium ea ad harum sed commodi.
                Id magni quo cupiditate asperiores eum et nobis et.", "Small Soft Mouse", "Harum consequuntur dolorem et ea dolore ducimus rerum.", new DateTime(2020, 4, 22, 16, 31, 38, 495, DateTimeKind.Local).AddTicks(4202), "https://domenic.info", 29 },
                    { 23, new DateTime(2020, 4, 20, 16, 31, 38, 543, DateTimeKind.Local).AddTicks(8850), @"Dignissimos beatae sed.
                Dignissimos distinctio eos occaecati debitis quisquam.
                Tempore nihil earum.
                Iste vero nisi quod.
                Totam molestiae incidunt autem dolorem incidunt atque.
                Voluptatem quisquam numquam.
                Blanditiis delectus ratione.
                Eos quo qui eos rerum sed dignissimos eum et.
                Et et voluptatum quis consequatur.
                Dignissimos qui qui sint pariatur voluptas est numquam.", "Ergonomic Concrete Computer", "Dolores quaerat sequi molestias.", new DateTime(2020, 4, 22, 16, 31, 38, 543, DateTimeKind.Local).AddTicks(9379), "https://rosalia.biz", 29 }
                });

            migrationBuilder.InsertData(
                table: "Collaborators",
                columns: new[] { "Id", "FullName", "ProjectId", "Role" },
                values: new object[,]
                {
                    { 2232, "Heath Turner", 5, "Principal Optimization Designer" },
                    { 2536, "Laurianne Miller", 20, "Product Factors Manager" },
                    { 3922, "Ramon Kuhic", 20, "Principal Mobility Associate" },
                    { 3488, "Lowell Walker", 11, "Regional Division Engineer" },
                    { 3558, "Guiseppe Abbott", 11, "Future Markets Engineer" },
                    { 1173, "Giovanna Flatley", 15, "Legacy Accountability Specialist" },
                    { 5364, "Flavie Jenkins", 15, "District Identity Consultant" },
                    { 121, "Melany Weissnat", 18, "Investor Web Administrator" },
                    { 3959, "Brisa Reinger", 18, "Direct Paradigm Producer" },
                    { 4478, "Hans Barton", 26, "Legacy Security Facilitator" },
                    { 5085, "Keara Gleason", 26, "Internal Usability Officer" },
                    { 8611, "Kelvin Bashirian", 17, "Chief Mobility Liaison" },
                    { 4991, "Cecile Hoppe", 17, "Direct Identity Supervisor" },
                    { 3641, "Monroe Borer", 3, "Senior Implementation Engineer" },
                    { 5340, "Alfonzo Nolan", 25, "Lead Infrastructure Specialist" },
                    { 5803, "Baby Torp", 14, "Dynamic Intranet Supervisor" },
                    { 2374, "Athena Wunsch", 14, "Internal Paradigm Engineer" },
                    { 5565, "Gaetano Feeney", 8, "Customer Configuration Associate" },
                    { 4206, "Mavis Moore", 8, "Corporate Functionality Strategist" },
                    { 5183, "Laney Kertzmann", 30, "Human Integration Technician" },
                    { 148, "Bobby Kassulke", 30, "National Accountability Facilitator" },
                    { 2294, "Lavern Waelchi", 6, "Central Configuration Planner" },
                    { 1918, "Nya Kautzer", 6, "Human Usability Strategist" },
                    { 5740, "Destin Ebert", 7, "Human Brand Executive" },
                    { 1968, "Irwin Stracke", 7, "Dynamic Security Engineer" },
                    { 266, "Jennings Hammes", 19, "District Optimization Supervisor" },
                    { 9246, "Tia Monahan", 19, "Customer Interactions Specialist" },
                    { 2540, "Yazmin Hahn", 25, "Investor Research Technician" },
                    { 822, "Brent Carroll", 3, "Product Branding Architect" },
                    { 7838, "Libby Wilkinson", 12, "Principal Integration Analyst" },
                    { 4408, "Aubree Zulauf", 12, "National Accounts Producer" },
                    { 2334, "Brooks Spencer", 5, "Global Infrastructure Orchestrator" },
                    { 8641, "Colton Ledner", 28, "District Solutions Assistant" },
                    { 5017, "Pedro Stracke", 28, "District Operations Manager" },
                    { 2420, "Cesar Schaden", 4, "Central Intranet Administrator" },
                    { 2982, "Judd Pfannerstill", 4, "Dynamic Functionality Agent" },
                    { 5139, "Jamey Keebler", 1, "Dynamic Mobility Director" },
                    { 9116, "Laura Walter", 1, "Dynamic Paradigm Specialist" },
                    { 4895, "Rico Olson", 9, "Direct Division Analyst" },
                    { 4658, "Evert Kovacek", 9, "Chief Group Manager" },
                    { 9931, "Marisa Heller", 13, "Central Group Engineer" },
                    { 4667, "Retha Walker", 13, "Corporate Accounts Assistant" },
                    { 9017, "Sadie Yost", 16, "Legacy Accounts Designer" },
                    { 3207, "Don Hartmann", 16, "Dynamic Accounts Technician" },
                    { 8157, "Araceli Ortiz", 22, "National Directives Architect" },
                    { 4167, "Hipolito Schowalter", 22, "Investor Identity Supervisor" },
                    { 3914, "Zackery Champlin", 24, "Principal Web Liaison" },
                    { 729, "Trent Parker", 24, "National Applications Facilitator" },
                    { 3237, "Ismael Schaden", 27, "Dynamic Infrastructure Architect" },
                    { 3571, "Jamir Olson", 27, "National Metrics Developer" },
                    { 1119, "Ethan Steuber", 29, "Internal Creative Agent" },
                    { 6040, "Mariano Bernier", 29, "Central Assurance Executive" },
                    { 8424, "Linnea Connelly", 2, "Dynamic Marketing Director" },
                    { 7450, "Osborne Goyette", 2, "Regional Web Agent" },
                    { 6142, "Marty Nicolas", 21, "Chief Data Technician" },
                    { 9353, "Newell Frami", 21, "Product Interactions Engineer" },
                    { 3272, "Francisca Bergstrom", 10, "Legacy Tactics Executive" },
                    { 7516, "Abdul Lakin", 10, "Dynamic Interactions Analyst" },
                    { 8021, "Blaze Dietrich", 23, "Global Assurance Analyst" },
                    { 2355, "Annalise Roob", 23, "Future Data Assistant" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmbeddedProject_UserId",
                table: "EmbeddedProject",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmbeddedProject_User_UserId",
                table: "EmbeddedProject",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmbeddedProject_User_UserId",
                table: "EmbeddedProject");

            migrationBuilder.DropIndex(
                name: "IX_EmbeddedProject_UserId",
                table: "EmbeddedProject");

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 266);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 729);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 822);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 1119);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 1173);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 1918);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 1968);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 2232);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 2294);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 2334);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 2355);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 2374);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 2420);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 2536);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 2540);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 2982);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 3207);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 3237);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 3272);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 3488);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 3558);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 3571);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 3641);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 3914);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 3922);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 3959);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 4167);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 4206);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 4408);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 4478);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 4658);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 4667);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 4895);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 4991);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 5017);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 5085);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 5139);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 5183);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 5340);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 5364);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 5565);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 5740);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 5803);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 6040);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 6142);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 7450);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 7516);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 7838);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 8021);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 8157);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 8424);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 8611);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 8641);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 9017);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 9116);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 9246);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 9353);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 9931);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EmbeddedProject");
        }
    }
}
