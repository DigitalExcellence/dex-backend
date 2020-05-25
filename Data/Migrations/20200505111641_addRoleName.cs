using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _4_Data.Migrations
{
    public partial class addRoleName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleScope_Role_Roleid",
                table: "RoleScope");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_Roleid",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "Roleid",
                table: "User",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_User_Roleid",
                table: "User",
                newName: "IX_User_RoleId");

            migrationBuilder.RenameColumn(
                name: "Roleid",
                table: "RoleScope",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleScope_Roleid",
                table: "RoleScope",
                newName: "IX_RoleScope_RoleId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Role",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Role",
                nullable: true);

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "IdentityId", "Name", "ProfileUrl", "RoleId" },
                values: new object[,]
                {
                    { 1, "Fred_Hackett26@yahoo.com", "2", "Brett", null, null },
                    { 28, "Melvina78@yahoo.com", "29", "Gerard", null, null },
                    { 27, "Ezequiel_Wilderman@gmail.com", "28", "Vinnie", null, null },
                    { 26, "Russell_Lakin@yahoo.com", "27", "Arielle", null, null },
                    { 25, "Chelsea.Schroeder97@hotmail.com", "26", "Sylvan", null, null },
                    { 24, "Alvera.Gottlieb@gmail.com", "25", "Lonzo", null, null },
                    { 23, "Adrien.Collier@hotmail.com", "24", "Oleta", null, null },
                    { 22, "Javon74@hotmail.com", "23", "Hans", null, null },
                    { 21, "Agustina61@hotmail.com", "22", "Sienna", null, null },
                    { 20, "Cristian5@yahoo.com", "21", "Ava", null, null },
                    { 19, "Jacky79@yahoo.com", "20", "Anya", null, null },
                    { 18, "Vincenza36@gmail.com", "19", "Noah", null, null },
                    { 17, "Tatum.Hyatt@hotmail.com", "18", "Ernestine", null, null },
                    { 16, "Drew_Monahan19@gmail.com", "17", "Freda", null, null },
                    { 15, "Dasia_Nitzsche@yahoo.com", "16", "Laurie", null, null },
                    { 14, "Karlie54@yahoo.com", "15", "Marlene", null, null },
                    { 13, "Hillary.Tillman@yahoo.com", "14", "Alanna", null, null },
                    { 12, "Melany89@yahoo.com", "13", "Justyn", null, null },
                    { 11, "Nicola.Stamm@hotmail.com", "12", "Delaney", null, null },
                    { 10, "Myrtie.Weber34@hotmail.com", "11", "Pearlie", null, null },
                    { 9, "Tessie.Morar72@hotmail.com", "10", "Annalise", null, null },
                    { 8, "Charity92@hotmail.com", "9", "Markus", null, null },
                    { 7, "Elisha_Dietrich4@yahoo.com", "8", "Kolby", null, null },
                    { 6, "Carli_Kling43@gmail.com", "7", "Savanna", null, null },
                    { 5, "Polly61@yahoo.com", "6", "Richmond", null, null },
                    { 4, "Louvenia.Wolff@yahoo.com", "5", "Jakayla", null, null },
                    { 3, "Filomena.Gerhold13@gmail.com", "4", "Tamara", null, null },
                    { 2, "Nolan86@hotmail.com", "3", "Myah", null, null },
                    { 29, "Viola1@gmail.com", "30", "Vince", null, null },
                    { 30, "Mertie43@yahoo.com", "31", "Leonor", null, null }
                });

            migrationBuilder.InsertData(
                table: "Project",
                columns: new[] { "Id", "Created", "Description", "Name", "ShortDescription", "Updated", "Uri", "UserId" },
                values: new object[,]
                {
                    { 3, new DateTime(2020, 5, 3, 13, 16, 40, 353, DateTimeKind.Local).AddTicks(6093), @"Dolorem aspernatur sit sequi.
                Beatae et exercitationem voluptatum cum.
                Aliquid vero omnis.
                Fuga provident voluptatem illum.
                Commodi sapiente tempora in aspernatur.
                Ut velit qui provident id omnis.
                Architecto vitae aspernatur harum est.
                Et ullam magni consequatur excepturi explicabo et et.
                Mollitia iusto beatae.
                Sint consequatur nulla.", "Ergonomic Plastic Computer", "Officiis et illo voluptates.", new DateTime(2020, 5, 5, 13, 16, 40, 353, DateTimeKind.Local).AddTicks(6124), "https://andy.name", 2 },
                    { 30, new DateTime(2020, 5, 3, 13, 16, 40, 451, DateTimeKind.Local).AddTicks(9069), @"Quos voluptate voluptas quas voluptates dolorum impedit quas perferendis impedit.
                In explicabo doloremque maxime placeat incidunt.
                Neque vitae autem eos sint non.
                In id exercitationem suscipit et perspiciatis et.
                Eius itaque tempore aliquid voluptatem molestiae reiciendis excepturi.
                Qui veritatis ut laudantium eligendi autem provident aspernatur magni eveniet.
                Voluptatem quia harum quos repudiandae ut molestias perferendis velit ipsum.
                Neque culpa libero officiis explicabo quasi qui non occaecati.
                Sunt voluptas eaque vero.
                Quasi impedit tempora omnis ut aut est voluptates culpa perferendis.", "Incredible Wooden Bike", "Sit odio illo quam delectus.", new DateTime(2020, 5, 5, 13, 16, 40, 451, DateTimeKind.Local).AddTicks(9112), "http://rosamond.info", 25 },
                    { 28, new DateTime(2020, 5, 3, 13, 16, 40, 446, DateTimeKind.Local).AddTicks(8115), @"Atque molestiae magnam aperiam iste dolor et.
                Incidunt natus sequi reiciendis.
                Consequatur assumenda sit blanditiis unde et iure expedita harum qui.
                Cumque occaecati incidunt unde quis.
                Assumenda commodi quos velit quisquam necessitatibus illo nisi qui.
                Sed placeat veniam sit.
                Perspiciatis voluptates impedit dolorem pariatur autem sed voluptatem quis necessitatibus.
                Odit delectus suscipit aperiam rerum ipsam facere rem autem.
                Corrupti cupiditate sunt aut sapiente fugit voluptatem corporis pariatur.
                Aliquid quidem nisi sequi laborum occaecati eum cupiditate delectus et.", "Handcrafted Granite Cheese", "Sed cupiditate et dolor dicta.", new DateTime(2020, 5, 5, 13, 16, 40, 446, DateTimeKind.Local).AddTicks(8144), "https://samantha.biz", 25 },
                    { 21, new DateTime(2020, 5, 3, 13, 16, 40, 427, DateTimeKind.Local).AddTicks(7620), @"Et suscipit ex modi nihil ab adipisci minus ab.
                Ipsa illo enim.
                Repellendus assumenda voluptates.
                Dolor eaque omnis.
                Soluta autem iste officiis placeat velit.
                Est unde laborum minus officia sit et at.
                Qui distinctio quos est fuga hic pariatur.
                Voluptates repellendus nesciunt totam porro quis magnam non sunt quam.
                Voluptatibus nam eum corporis repellendus et in dolores.
                Voluptatem est aspernatur quisquam repudiandae vel.", "Sleek Steel Bike", "Quia unde fugit vero minus cupiditate soluta delectus delectus magnam.", new DateTime(2020, 5, 5, 13, 16, 40, 427, DateTimeKind.Local).AddTicks(7650), "https://david.name", 22 },
                    { 9, new DateTime(2020, 5, 3, 13, 16, 40, 369, DateTimeKind.Local).AddTicks(5973), @"Qui similique sit esse facere.
                Aliquid nobis aut laborum molestiae.
                Excepturi blanditiis ullam corrupti facilis.
                Architecto non sequi officia.
                A voluptatem voluptas.
                Asperiores aut libero ullam deserunt vero consequatur nihil.
                Non incidunt ut sint rerum.
                Optio corporis et id doloremque dolor hic ut.
                Quaerat voluptas alias voluptatem qui perspiciatis modi.
                Maiores expedita ea quo.", "Fantastic Cotton Hat", "Enim quo praesentium molestias.", new DateTime(2020, 5, 5, 13, 16, 40, 369, DateTimeKind.Local).AddTicks(6008), "http://gisselle.biz", 22 },
                    { 13, new DateTime(2020, 5, 3, 13, 16, 40, 392, DateTimeKind.Local).AddTicks(8644), @"Mollitia est officia quaerat deserunt et iusto eveniet cumque quaerat.
                Aspernatur similique voluptates aliquam rem ea animi vel aliquid modi.
                Vero velit quidem sed.
                Saepe quam iusto nobis.
                Non et eum et veritatis est quis modi veritatis.
                Rerum dolorem eaque sequi nam voluptates.
                In earum est fugiat eum expedita ducimus placeat.
                Fuga illo corporis perspiciatis deleniti ut.
                Consectetur sunt et non facere velit harum.
                Sequi quis dolor molestias est.", "Incredible Concrete Fish", "Atque officiis quis dolore.", new DateTime(2020, 5, 5, 13, 16, 40, 392, DateTimeKind.Local).AddTicks(8695), "https://alyson.info", 20 },
                    { 29, new DateTime(2020, 5, 3, 13, 16, 40, 449, DateTimeKind.Local).AddTicks(2544), @"Voluptatem inventore eum dolorum quis necessitatibus.
                Delectus quisquam occaecati reiciendis quas totam fuga dolor.
                Molestiae recusandae incidunt quas qui qui.
                Repellat modi aspernatur officiis odit.
                Sed et sint ut dolore consectetur aspernatur tempora ut.
                Quidem veniam nobis.
                Animi perspiciatis commodi enim laudantium eum earum.
                Quis omnis nemo magnam optio est eum qui amet.
                In dolor id.
                Veritatis delectus eius et vitae adipisci cupiditate quo mollitia modi.", "Intelligent Frozen Bacon", "Quo assumenda blanditiis eos repellendus voluptate.", new DateTime(2020, 5, 5, 13, 16, 40, 449, DateTimeKind.Local).AddTicks(2577), "http://franco.biz", 16 },
                    { 24, new DateTime(2020, 5, 3, 13, 16, 40, 435, DateTimeKind.Local).AddTicks(1990), @"Sit veritatis recusandae nisi tenetur tempore est nam vel unde.
                Qui aut velit.
                Dolorum dicta dicta id omnis.
                Officiis numquam unde cumque fuga ut sapiente voluptatem rerum.
                Minus reprehenderit hic odit alias velit dolores error et iure.
                Et modi ut vel et esse sed qui incidunt.
                Aut quasi ratione blanditiis nulla eum rerum.
                Nihil est facilis accusamus culpa.
                Laboriosam veritatis explicabo ad quia tenetur.
                Corporis reprehenderit doloremque quaerat doloribus est rerum.", "Generic Metal Pizza", "Iste voluptas nostrum porro laudantium omnis voluptatem aut.", new DateTime(2020, 5, 5, 13, 16, 40, 435, DateTimeKind.Local).AddTicks(2019), "http://fay.biz", 16 },
                    { 8, new DateTime(2020, 5, 3, 13, 16, 40, 367, DateTimeKind.Local).AddTicks(1991), @"Ipsum sit odit eaque totam sequi aut.
                Tempore ipsa est rerum enim consectetur enim odio consectetur.
                Commodi fugiat magnam ab explicabo.
                Totam corporis numquam porro et commodi magni.
                Ad totam quas voluptatem ab debitis.
                Molestias iusto animi veritatis dolore mollitia illo est ullam quam.
                Qui necessitatibus nesciunt aut nostrum sed et occaecati.
                Dolorem earum aliquam sit nemo repudiandae et.
                Nulla nobis doloribus iure enim.
                Vero qui est doloribus pariatur asperiores.", "Intelligent Granite Fish", "In sint exercitationem et ut labore.", new DateTime(2020, 5, 5, 13, 16, 40, 367, DateTimeKind.Local).AddTicks(2023), "https://cade.com", 15 },
                    { 22, new DateTime(2020, 5, 3, 13, 16, 40, 430, DateTimeKind.Local).AddTicks(2979), @"Velit accusamus accusamus enim ea nihil qui.
                Blanditiis esse iste sit.
                Odio omnis laboriosam velit magni distinctio reiciendis natus.
                Blanditiis maiores et suscipit provident eos totam voluptatem quasi quibusdam.
                Dolores nemo quae excepturi.
                Omnis voluptas non harum ad odit quia dolor.
                Vel est deserunt omnis ut.
                Occaecati ipsa quos esse aut dignissimos voluptatem quia quisquam.
                Dolores ut possimus tenetur consectetur magni molestias eum omnis.
                Qui ducimus molestiae facilis ut.", "Fantastic Granite Shoes", "Vitae inventore porro numquam officiis natus quo itaque impedit.", new DateTime(2020, 5, 5, 13, 16, 40, 430, DateTimeKind.Local).AddTicks(3012), "http://torey.biz", 14 },
                    { 16, new DateTime(2020, 5, 3, 13, 16, 40, 403, DateTimeKind.Local).AddTicks(9980), @"Et est temporibus vitae consequatur quia ducimus aut voluptatibus.
                Quidem at omnis molestias qui.
                Cum perferendis perspiciatis est vitae culpa iste non.
                Quos quo nesciunt laudantium est ea ducimus quo possimus totam.
                Minus aut quibusdam quo nam aut.
                Assumenda autem illo sequi iusto praesentium amet consectetur nisi quisquam.
                Impedit modi magni.
                Ratione officiis velit nostrum nisi quaerat sapiente.
                Eos autem ipsa accusantium.
                Sunt numquam nihil.", "Fantastic Concrete Fish", "Aut libero repellendus.", new DateTime(2020, 5, 5, 13, 16, 40, 404, DateTimeKind.Local).AddTicks(43), "https://tomasa.org", 14 },
                    { 17, new DateTime(2020, 5, 3, 13, 16, 40, 411, DateTimeKind.Local).AddTicks(3893), @"Inventore qui nihil ab dolore exercitationem nesciunt.
                Molestiae molestiae autem voluptas et aut sit repudiandae ea qui.
                Quia et magni perferendis tenetur minus.
                Ut id ut sed dolores deserunt quasi.
                Et laudantium iusto maxime quibusdam minus at et.
                Ad nobis non perferendis reiciendis.
                Delectus aut aut voluptatem aspernatur repudiandae non quo.
                Ipsam eligendi rerum nihil est reprehenderit exercitationem dolor optio.
                Ut quisquam deleniti et animi optio quia alias voluptas quod.
                Cupiditate ut consequatur vitae ullam.", "Gorgeous Metal Ball", "Magnam voluptas ut vel aut placeat nobis.", new DateTime(2020, 5, 5, 13, 16, 40, 411, DateTimeKind.Local).AddTicks(3965), "https://shanelle.info", 12 },
                    { 18, new DateTime(2020, 5, 3, 13, 16, 40, 417, DateTimeKind.Local).AddTicks(2758), @"Et sint dicta beatae recusandae corrupti ea veniam similique omnis.
                Dicta aut atque autem voluptatem culpa laboriosam iure eum ipsa.
                Est fugit molestiae aut autem consectetur.
                Voluptates saepe architecto earum possimus commodi.
                Qui nostrum voluptatem.
                Sed ut omnis consectetur eveniet rerum omnis sed harum.
                Quia sunt ut quasi earum qui temporibus aut voluptatem a.
                Dolorem temporibus expedita aut voluptas ratione numquam quia.
                Optio nostrum consectetur in et quasi est sed numquam.
                Adipisci deserunt iste.", "Practical Frozen Salad", "Nihil aut provident officiis illum voluptates modi.", new DateTime(2020, 5, 5, 13, 16, 40, 417, DateTimeKind.Local).AddTicks(2814), "http://belle.net", 10 },
                    { 10, new DateTime(2020, 5, 3, 13, 16, 40, 373, DateTimeKind.Local).AddTicks(1333), @"Molestiae necessitatibus porro sunt expedita id saepe quae quasi dolores.
                Voluptatem vero at.
                Maxime doloribus architecto dolore vero veniam sit.
                Asperiores doloremque ratione cupiditate doloremque earum.
                Sed commodi assumenda fugit est magnam dolorem magni.
                Magnam omnis enim rerum beatae ut.
                Animi et amet voluptatem quis laboriosam facilis.
                Id velit ea est veritatis excepturi non et.
                Dignissimos perferendis soluta.
                Quod magni ipsam et vero in.", "Practical Fresh Towels", "Explicabo ut autem ea est blanditiis perspiciatis accusamus nulla soluta.", new DateTime(2020, 5, 5, 13, 16, 40, 373, DateTimeKind.Local).AddTicks(1381), "https://addie.biz", 10 },
                    { 15, new DateTime(2020, 5, 3, 13, 16, 40, 400, DateTimeKind.Local).AddTicks(7013), @"Omnis deserunt ipsam exercitationem voluptatum accusantium.
                Unde corrupti sed odit minus.
                Quidem fugit totam voluptatem ea numquam vel quisquam.
                Velit est sed voluptas in temporibus.
                Autem perferendis molestiae similique omnis.
                Velit sunt consequuntur ut nemo aut omnis.
                Ut vero dolor quas quo repellat voluptatum.
                Tempora nulla sed expedita quasi reprehenderit officia omnis.
                Qui unde enim fugit occaecati sit dolorem sapiente.
                Repudiandae saepe dolores.", "Awesome Steel Chair", "Voluptates impedit rerum.", new DateTime(2020, 5, 5, 13, 16, 40, 400, DateTimeKind.Local).AddTicks(7067), "http://tyler.org", 9 },
                    { 5, new DateTime(2020, 5, 3, 13, 16, 40, 359, DateTimeKind.Local).AddTicks(8760), @"Accusantium minima et.
                Aut dolor ea voluptatem est facere eos ullam est.
                Impedit hic asperiores placeat.
                Et aspernatur debitis vitae sapiente.
                Quis fugiat aperiam iste nostrum ut ut illo consequatur saepe.
                Consequatur ab amet et at ut.
                Optio doloribus dicta numquam ut aliquid ex culpa aut.
                Non itaque voluptatem velit.
                Debitis excepturi rerum voluptas voluptas voluptatem non et et.
                Voluptatem magnam qui rerum error aliquid quasi et quasi eum.", "Handmade Granite Keyboard", "Consequatur sequi nostrum nobis velit quo error eaque voluptas accusamus.", new DateTime(2020, 5, 5, 13, 16, 40, 359, DateTimeKind.Local).AddTicks(8807), "http://elody.name", 8 },
                    { 26, new DateTime(2020, 5, 3, 13, 16, 40, 441, DateTimeKind.Local).AddTicks(8394), @"Modi tenetur veniam ex ad nihil fuga temporibus repellat.
                Quos possimus autem cum iure aut distinctio.
                Quia rerum rerum quis.
                Sit aut ut.
                Dicta voluptatem explicabo ipsam quae dolor.
                Iure reprehenderit blanditiis natus provident dolorem dicta nulla.
                Omnis beatae labore doloremque numquam laboriosam temporibus.
                Qui reiciendis accusantium itaque at.
                Ut voluptatum quis aut sed assumenda ut nostrum dignissimos reiciendis.
                Quis asperiores molestiae.", "Handmade Wooden Salad", "Mollitia aut quibusdam.", new DateTime(2020, 5, 5, 13, 16, 40, 441, DateTimeKind.Local).AddTicks(8425), "https://marcelino.org", 7 },
                    { 12, new DateTime(2020, 5, 3, 13, 16, 40, 388, DateTimeKind.Local).AddTicks(5503), @"Culpa quaerat voluptatem voluptatibus dicta.
                Molestias quisquam inventore quidem error illum ad.
                Vel veritatis repellat quam amet ut incidunt qui cumque.
                Optio ut et quod nihil voluptatem ipsum repellendus aut.
                Officia iusto dicta saepe est.
                Ex excepturi incidunt commodi.
                Enim sint commodi excepturi necessitatibus commodi.
                Iusto deleniti aspernatur.
                Natus quos veniam sit saepe voluptas perferendis autem totam.
                Aut delectus voluptatem earum reiciendis odit est aperiam aut.", "Awesome Rubber Fish", "Et suscipit nisi molestiae maiores aliquam incidunt illum.", new DateTime(2020, 5, 5, 13, 16, 40, 388, DateTimeKind.Local).AddTicks(5568), "https://van.info", 7 },
                    { 25, new DateTime(2020, 5, 3, 13, 16, 40, 439, DateTimeKind.Local).AddTicks(297), @"Fugit placeat rerum cupiditate voluptatum nulla quia nulla.
                Ad voluptatibus libero soluta fugit velit vero.
                Possimus laborum voluptatem aut ex voluptatem sed dolorum.
                Quaerat dignissimos sed.
                Praesentium nostrum in est.
                Molestias ut consequuntur velit officiis incidunt repellat voluptas odio deleniti.
                Quidem est ducimus.
                Quis minus dolorum in recusandae.
                Mollitia cupiditate ut blanditiis officia debitis est nisi vel.
                Explicabo doloremque velit eligendi molestias laborum ut quod.", "Sleek Concrete Fish", "Aut officia quo quidem provident eum similique repellat qui saepe.", new DateTime(2020, 5, 5, 13, 16, 40, 439, DateTimeKind.Local).AddTicks(360), "https://candido.com", 6 },
                    { 6, new DateTime(2020, 5, 3, 13, 16, 40, 362, DateTimeKind.Local).AddTicks(3239), @"Rem quos corrupti commodi nulla suscipit minima.
                Numquam quidem velit.
                Qui architecto beatae vel accusamus quasi ex expedita facilis.
                Id facilis officia in aut occaecati asperiores.
                Assumenda necessitatibus vel ut.
                Culpa eveniet est sapiente illum perferendis.
                Maiores beatae sequi velit cumque autem nam.
                Aspernatur odit voluptatem voluptatem rerum et.
                Voluptas est dignissimos voluptas omnis aut eaque et cupiditate praesentium.
                Nobis et sint quaerat odit sed rerum.", "Rustic Metal Chips", "Culpa non quos.", new DateTime(2020, 5, 5, 13, 16, 40, 362, DateTimeKind.Local).AddTicks(3269), "https://desiree.org", 6 },
                    { 14, new DateTime(2020, 5, 3, 13, 16, 40, 396, DateTimeKind.Local).AddTicks(9863), @"Placeat tempore voluptatem ut sint occaecati sapiente quis mollitia nulla.
                Neque vel necessitatibus neque nemo velit cum.
                Officiis itaque inventore.
                Qui sit sit ullam.
                Est est voluptatem molestiae sunt deserunt consequatur quod.
                Beatae labore assumenda illum dolores explicabo.
                Eveniet impedit et ex soluta.
                Unde magnam ab sed.
                Est dignissimos aut ab ut modi voluptas animi maiores culpa.
                Architecto temporibus rerum quam.", "Unbranded Wooden Shirt", "Suscipit odio voluptas illo et delectus voluptates et voluptatum.", new DateTime(2020, 5, 5, 13, 16, 40, 396, DateTimeKind.Local).AddTicks(9920), "https://jermain.org", 4 },
                    { 7, new DateTime(2020, 5, 3, 13, 16, 40, 364, DateTimeKind.Local).AddTicks(7201), @"Similique veritatis expedita corrupti eligendi illo quo occaecati.
                Aliquam deleniti hic non et id.
                Aut dolorem cupiditate.
                Alias dolor cum dolorem voluptas ex aliquid eum maxime.
                Et omnis a.
                A in earum velit.
                Eos nihil officia quae iure culpa harum voluptatem eos.
                Animi aut est doloremque et nam repellendus eum.
                Nesciunt laboriosam nihil qui est autem ut non doloribus.
                Tempore laboriosam nesciunt.", "Refined Rubber Car", "Ea nam velit esse minus quis dolore eligendi eaque corrupti.", new DateTime(2020, 5, 5, 13, 16, 40, 364, DateTimeKind.Local).AddTicks(7236), "http://maximo.biz", 4 },
                    { 23, new DateTime(2020, 5, 3, 13, 16, 40, 432, DateTimeKind.Local).AddTicks(7595), @"Consequatur officiis qui omnis commodi eum sunt nobis.
                Voluptas alias et eaque.
                Fugit pariatur et.
                Commodi delectus blanditiis.
                Rerum in excepturi illum dolore minus ipsam.
                Sed iste fuga voluptatem quidem ab quas officia et aliquam.
                Aperiam architecto aut natus ut ut quidem laboriosam delectus error.
                Nulla omnis numquam corporis doloribus similique dolore.
                Culpa ex quis quaerat natus dignissimos labore.
                Iusto veritatis ut voluptatum occaecati odit repellat nihil et est.", "Generic Concrete Fish", "Perspiciatis sint laborum.", new DateTime(2020, 5, 5, 13, 16, 40, 432, DateTimeKind.Local).AddTicks(7626), "http://rashawn.name", 3 },
                    { 20, new DateTime(2020, 5, 3, 13, 16, 40, 425, DateTimeKind.Local).AddTicks(1593), @"Et delectus neque dolorem id nulla qui.
                Deserunt corrupti quia.
                Dignissimos dolorem hic molestiae doloribus velit fugiat.
                Facilis velit labore voluptatem consequatur veritatis.
                Sapiente ut earum magni numquam exercitationem quam.
                Ut doloribus et aut dolorum quo et.
                Omnis rerum voluptas et nobis doloremque placeat eaque.
                Reiciendis nam rerum quos.
                Qui neque cum ut corrupti explicabo est ea.
                Minima voluptatem facilis reiciendis iusto et in non possimus nulla.", "Generic Soft Keyboard", "Nobis vel cum asperiores saepe non eius ut.", new DateTime(2020, 5, 5, 13, 16, 40, 425, DateTimeKind.Local).AddTicks(1644), "https://chase.com", 3 },
                    { 19, new DateTime(2020, 5, 3, 13, 16, 40, 421, DateTimeKind.Local).AddTicks(8263), @"Enim inventore quod.
                Beatae at quaerat sint eveniet delectus dignissimos.
                Omnis aliquam possimus aut voluptas odit est dolore hic.
                Nemo ad in.
                Natus numquam voluptatem repellat.
                Deserunt rerum qui modi nesciunt ullam.
                Excepturi nihil veniam temporibus et similique molestiae rerum.
                Ut est accusantium molestiae.
                Molestiae itaque sequi.
                Placeat totam neque et.", "Rustic Steel Mouse", "Ipsa voluptas ut molestias pariatur.", new DateTime(2020, 5, 5, 13, 16, 40, 421, DateTimeKind.Local).AddTicks(8332), "http://brock.biz", 3 },
                    { 4, new DateTime(2020, 5, 3, 13, 16, 40, 356, DateTimeKind.Local).AddTicks(7524), @"Repellendus optio ab doloribus consequatur eum consectetur repellat incidunt voluptatem.
                Corporis animi qui quaerat vel ratione blanditiis sed.
                Unde tempora debitis tempora.
                Libero minus saepe.
                Quam accusamus odio est quas accusamus.
                Unde dignissimos enim est quia vel.
                Illo nesciunt enim blanditiis dolor voluptatum sit.
                Atque sed architecto dignissimos natus illum qui.
                Eos et ullam provident vel veniam ad provident explicabo eos.
                Ut cumque amet nulla et id est dolorem quasi.", "Unbranded Steel Gloves", "Numquam omnis quam ducimus at eum et optio.", new DateTime(2020, 5, 5, 13, 16, 40, 356, DateTimeKind.Local).AddTicks(7572), "https://rigoberto.net", 3 },
                    { 1, new DateTime(2020, 5, 3, 13, 16, 40, 344, DateTimeKind.Local).AddTicks(8519), @"Laudantium qui dolores corrupti beatae quis et qui qui dolorem.
                Velit ea illo accusantium maxime commodi perferendis praesentium laboriosam nulla.
                Aliquam quis beatae rerum et aliquid iusto fugiat tempore esse.
                Dolorum nam iure dolorem in rerum.
                Quia molestiae numquam velit assumenda.
                Et iure in veniam est optio atque aut.
                Similique qui pariatur.
                Dolores vel porro voluptatem natus quos.
                Impedit quod iure rerum sint repellendus magni.
                Eius esse soluta illo.", "Handmade Frozen Bike", "Reprehenderit vitae occaecati est sequi ratione reiciendis.", new DateTime(2020, 5, 5, 13, 16, 40, 348, DateTimeKind.Local).AddTicks(3682), "https://alda.com", 3 },
                    { 27, new DateTime(2020, 5, 3, 13, 16, 40, 444, DateTimeKind.Local).AddTicks(3417), @"Placeat ut possimus qui.
                Facere maxime sed.
                Consectetur dicta neque consectetur unde beatae harum.
                Harum voluptatem et non ut commodi.
                Doloribus ut tempora eveniet.
                Culpa quia quo.
                Id temporibus provident ratione natus vel.
                Consequatur facere deleniti ut.
                Et illum magnam.
                Quia vel nemo vel hic nemo ea officiis excepturi quod.", "Unbranded Metal Pizza", "Cupiditate nulla occaecati.", new DateTime(2020, 5, 5, 13, 16, 40, 444, DateTimeKind.Local).AddTicks(3447), "http://rodolfo.biz", 2 },
                    { 2, new DateTime(2020, 5, 3, 13, 16, 40, 351, DateTimeKind.Local).AddTicks(1130), @"Facere tenetur ea minima.
                Impedit et reiciendis quia repellat itaque consequatur ut.
                Ex distinctio et rerum sunt sit voluptatem.
                Vero earum repellat.
                Excepturi deserunt aspernatur consectetur nostrum qui.
                Et vel dolor vel eos voluptatem ut ad velit a.
                Quo temporibus dolore velit in rerum eveniet.
                Sequi facilis qui cumque.
                Laudantium veniam dolore.
                Est debitis recusandae impedit.", "Ergonomic Concrete Chips", "Molestiae veniam amet dolor pariatur excepturi et.", new DateTime(2020, 5, 5, 13, 16, 40, 351, DateTimeKind.Local).AddTicks(1186), "https://leonard.name", 26 },
                    { 11, new DateTime(2020, 5, 3, 13, 16, 40, 382, DateTimeKind.Local).AddTicks(2481), @"Aut et maiores eaque eveniet quia adipisci et officia.
                Eos cupiditate officiis.
                Molestiae omnis est id quidem.
                Alias alias est.
                Nisi voluptatem nihil aut ut a quam ut illum totam.
                Temporibus deserunt tenetur velit voluptas dolor laboriosam dolores autem.
                Nisi ut laboriosam repudiandae sit quasi totam officiis.
                Nulla aut praesentium perferendis optio.
                Dolor explicabo fugiat et expedita fuga impedit ea cupiditate quidem.
                Eligendi voluptate officiis vero.", "Ergonomic Granite Gloves", "Et incidunt quis repellat ab dicta consectetur numquam.", new DateTime(2020, 5, 5, 13, 16, 40, 382, DateTimeKind.Local).AddTicks(2547), "http://osbaldo.net", 28 }
                });

            migrationBuilder.InsertData(
                table: "Collaborators",
                columns: new[] { "Id", "FullName", "ProjectId", "Role" },
                values: new object[,]
                {
                    { 5149, "Marcia Bogisich", 3, "Internal Data Agent" },
                    { 5015, "Earlene Macejkovic", 18, "International Communications Coordinator" },
                    { 1731, "Audrey Orn", 18, "Principal Accounts Executive" },
                    { 5719, "Rylee Prohaska", 17, "Forward Solutions Supervisor" },
                    { 7548, "Israel Will", 17, "Forward Infrastructure Orchestrator" },
                    { 3233, "Marcos Stark", 16, "Direct Data Coordinator" },
                    { 1359, "Bartholome Hoeger", 16, "Senior Research Coordinator" },
                    { 919, "Christop Brown", 22, "Future Paradigm Developer" },
                    { 6569, "Hellen Wilkinson", 22, "Principal Branding Analyst" },
                    { 3395, "Janae Abernathy", 8, "Central Program Supervisor" },
                    { 2241, "Lavonne Marvin", 8, "Internal Infrastructure Representative" },
                    { 2257, "Jermaine Windler", 24, "Chief Markets Producer" },
                    { 5241, "Jadyn Wolff", 24, "Dynamic Interactions Agent" },
                    { 1492, "Lindsey Rolfson", 10, "District Integration Engineer" },
                    { 8283, "Mertie Schuster", 29, "National Mobility Manager" },
                    { 9247, "Ottis Parker", 13, "District Quality Representative" },
                    { 1799, "Akeem Kuhn", 13, "Global Branding Officer" },
                    { 4459, "Modesto Schaefer", 9, "District Metrics Analyst" },
                    { 9003, "Filiberto Brekke", 9, "Human Security Designer" },
                    { 8159, "Harold Kuphal", 21, "Principal Accountability Supervisor" },
                    { 6940, "Korbin Block", 21, "Investor Research Technician" },
                    { 687, "Karina Torphy", 28, "Customer Accountability Agent" },
                    { 1674, "Dax McLaughlin", 28, "Future Accountability Technician" },
                    { 1536, "Fernando Keebler", 30, "Customer Division Architect" },
                    { 4212, "Vincenza Herzog", 30, "National Mobility Executive" },
                    { 8119, "Kirstin Hettinger", 2, "Investor Web Representative" },
                    { 8988, "Pink Schaden", 2, "Dynamic Factors Technician" },
                    { 4849, "Guy Bosco", 29, "Regional Quality Facilitator" },
                    { 7123, "Euna Bradtke", 10, "District Interactions Liaison" },
                    { 9072, "Edison Balistreri", 15, "Internal Marketing Specialist" },
                    { 8901, "Arjun Kiehn", 15, "Human Applications Consultant" },
                    { 9554, "Carlos Bradtke", 3, "Dynamic Applications Associate" },
                    { 7406, "Monserrat Bashirian", 27, "Dynamic Group Officer" },
                    { 5354, "Itzel Gorczany", 27, "Senior Identity Planner" },
                    { 990, "Orion Beatty", 1, "Human Markets Consultant" },
                    { 8184, "Edd Kohler", 1, "International Factors Coordinator" },
                    { 9885, "Verner Mann", 4, "Product Accountability Representative" },
                    { 3000, "Kadin Dickens", 4, "Future Security Supervisor" },
                    { 7030, "Flossie Weimann", 19, "Customer Tactics Producer" },
                    { 4935, "Rolando Williamson", 19, "Senior Tactics Orchestrator" },
                    { 655, "Aubrey Graham", 20, "Principal Security Developer" },
                    { 3317, "Jonathon Champlin", 20, "Future Communications Analyst" },
                    { 947, "Chaz Zboncak", 23, "Principal Branding Engineer" },
                    { 9682, "Jamil Rogahn", 23, "District Mobility Representative" },
                    { 3880, "Linda Upton", 7, "Internal Tactics Administrator" },
                    { 4714, "Deion Marvin", 7, "Senior Implementation Associate" },
                    { 9483, "Reilly Grady", 14, "Direct Accounts Designer" },
                    { 4732, "Mylene Welch", 14, "Central Creative Facilitator" },
                    { 7082, "Vicenta Kuphal", 6, "Customer Mobility Consultant" },
                    { 2762, "Laura Brakus", 6, "Product Communications Technician" },
                    { 5929, "Ayla Donnelly", 25, "District Intranet Officer" },
                    { 5605, "Ryleigh Rosenbaum", 25, "Principal Interactions Developer" },
                    { 6132, "Alvera Fritsch", 12, "Senior Directives Facilitator" },
                    { 9635, "Jana Hilpert", 12, "Central Accounts Developer" },
                    { 382, "Morgan VonRueden", 26, "Forward Web Technician" },
                    { 8951, "Manley Keeling", 26, "Dynamic Solutions Assistant" },
                    { 6515, "Mia Lowe", 5, "Central Marketing Orchestrator" },
                    { 1801, "Isobel Spinka", 5, "Regional Markets Producer" },
                    { 7600, "Percy Luettgen", 11, "Senior Brand Supervisor" },
                    { 1042, "Karlee Bednar", 11, "Principal Markets Orchestrator" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RoleScope_Role_RoleId",
                table: "RoleScope",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleScope_Role_RoleId",
                table: "RoleScope");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User");

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 382);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 655);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 687);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 919);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 947);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 990);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 1042);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 1359);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 1492);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 1536);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 1674);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 1731);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 1799);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 1801);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 2241);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 2257);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 2762);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 3000);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 3233);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 3317);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 3395);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 3880);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 4212);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 4459);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 4714);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 4732);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 4849);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 4935);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 5015);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 5149);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 5241);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 5354);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 5605);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 5719);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 5929);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 6132);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 6515);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 6569);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 6940);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 7030);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 7082);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 7123);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 7406);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 7548);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 7600);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 8119);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 8159);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 8184);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 8283);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 8901);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 8951);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 8988);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 9003);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 9072);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 9247);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 9483);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 9554);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 9635);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 9682);

            migrationBuilder.DeleteData(
                table: "Collaborators",
                keyColumn: "Id",
                keyValue: 9885);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 21);

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
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 29);

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
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 22);

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
                keyValue: 28);

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Role");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "User",
                newName: "Roleid");

            migrationBuilder.RenameIndex(
                name: "IX_User_RoleId",
                table: "User",
                newName: "IX_User_Roleid");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "RoleScope",
                newName: "Roleid");

            migrationBuilder.RenameIndex(
                name: "IX_RoleScope_RoleId",
                table: "RoleScope",
                newName: "IX_RoleScope_Roleid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Role",
                newName: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleScope_Role_Roleid",
                table: "RoleScope",
                column: "Roleid",
                principalTable: "Role",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_Roleid",
                table: "User",
                column: "Roleid",
                principalTable: "Role",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
