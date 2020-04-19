/*
* Digital Excellence Copyright (C) 2020 Brend Smits
* 
* This program is free software: you can redistribute it and/or modify 
* it under the terms of the GNU Lesser General Public License as published 
* by the Free Software Foundation version 3 of the License.
* 
* This program is distributed in the hope that it will be useful, 
* but WITHOUT ANY WARRANTY; without even the implied warranty 
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
* See the GNU Lesser General Public License for more details.
* 
* You can find a copy of the GNU Lesser General Public License 
* along with this program, in the LICENSE.md file in the root project directory.
* If not, see https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace _4_Data.Migrations
{

    public partial class Initial : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("User",
                                         table => new
                                                  {
                                                      Id = table.Column<int>(nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      Name = table.Column<string>(nullable: false),
                                                      Email = table.Column<string>(nullable: false),
                                                      IdentityId = table.Column<string>(nullable: false),
                                                      ProfileUrl = table.Column<string>(nullable: true)
                                                  },
                                         constraints: table => { table.PrimaryKey("PK_User", x => x.Id); });

            migrationBuilder.CreateTable("Project",
                                         table => new
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
                                             table.ForeignKey("FK_Project_User_UserId",
                                                              x => x.UserId,
                                                              "User",
                                                              "Id",
                                                              onDelete: ReferentialAction.Cascade);
                                         });

            migrationBuilder.CreateTable("Collaborators",
                                         table => new
                                                  {
                                                      Id = table.Column<int>(nullable: false)
                                                                .Annotation("SqlServer:Identity", "1, 1"),
                                                      FullName = table.Column<string>(nullable: true),
                                                      Role = table.Column<string>(nullable: true),
                                                      ProjectId = table.Column<int>(nullable: false)
                                                  },
                                         constraints: table =>
                                         {
                                             table.PrimaryKey("PK_Collaborators", x => x.Id);
                                             table.ForeignKey("FK_Collaborators_Project_ProjectId",
                                                              x => x.ProjectId,
                                                              "Project",
                                                              "Id",
                                                              onDelete: ReferentialAction.Cascade);
                                         });

            migrationBuilder.InsertData("User",
                                        new[] {"Id", "Email", "IdentityId", "Name", "ProfileUrl"},
                                        new object[,]
                                        {
                                            {1, "Stanton_Ullrich@hotmail.com", "2", "Melyssa", null},
                                            {28, "Freida_Maggio14@hotmail.com", "29", "Emmitt", null},
                                            {27, "Pearlie.Bartell@gmail.com", "28", "Myrtle", null},
                                            {26, "Wade_Cassin7@yahoo.com", "27", "Dagmar", null},
                                            {25, "Treva.Turcotte72@gmail.com", "26", "Lizeth", null},
                                            {24, "Dexter_Carter@gmail.com", "25", "Gregg", null},
                                            {23, "Benedict63@hotmail.com", "24", "Quinn", null},
                                            {22, "Gerhard92@gmail.com", "23", "Katelyn", null},
                                            {21, "Shakira72@gmail.com", "22", "Itzel", null},
                                            {20, "Grace_Balistreri52@yahoo.com", "21", "Vinnie", null},
                                            {19, "Felton53@gmail.com", "20", "Alden", null},
                                            {18, "Neva.Windler@gmail.com", "19", "Jesse", null},
                                            {17, "Kattie_Kunze@gmail.com", "18", "Nelle", null},
                                            {16, "Jeanne.Kreiger88@yahoo.com", "17", "Ella", null},
                                            {15, "Genoveva59@yahoo.com", "16", "Georgiana", null},
                                            {14, "Elissa.Lubowitz56@yahoo.com", "15", "Ayden", null},
                                            {13, "Haskell61@gmail.com", "14", "Jessica", null},
                                            {12, "Laurianne63@gmail.com", "13", "Ericka", null},
                                            {11, "Kenny.Wintheiser@hotmail.com", "12", "Ashlynn", null},
                                            {10, "Ramona47@gmail.com", "11", "Branson", null},
                                            {9, "Vergie75@hotmail.com", "10", "Isaiah", null},
                                            {8, "Carlie_Goyette17@hotmail.com", "9", "Adela", null},
                                            {7, "Nicholaus_Nikolaus72@yahoo.com", "8", "Ida", null},
                                            {6, "Lonnie74@yahoo.com", "7", "Pauline", null},
                                            {5, "Gay_Hermann34@hotmail.com", "6", "Beverly", null},
                                            {4, "Jaiden77@gmail.com", "5", "Tracy", null},
                                            {3, "Myriam.Kohler6@gmail.com", "4", "Jeff", null},
                                            {2, "Hellen35@yahoo.com", "3", "Rhett", null},
                                            {29, "Sydni66@yahoo.com", "30", "Godfrey", null},
                                            {30, "Maximilian.Schaden@yahoo.com", "31", "Tina", null}
                                        });

            migrationBuilder.InsertData("Project",
                                        new[]
                                        {
                                            "Id", "Created", "Description", "Name", "ShortDescription", "Updated",
                                            "Uri", "UserId"
                                        },
                                        new object[,]
                                        {
                                            {
                                                12,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 431, DateTimeKind.Local).AddTicks(
                                                    7048),
                                                @"Sit aut voluptatem repudiandae dolor ipsa sit ipsum voluptatem.
                Itaque et tempore ratione.
                Debitis fugiat sint cum esse veritatis qui itaque laboriosam quia.
                Sed molestiae doloribus ab iste quo itaque molestias voluptatem.
                Corrupti rerum explicabo totam ducimus veniam quasi itaque.
                Nesciunt iusto vero eius voluptatem quam delectus.
                Quia omnis ea.
                Voluptatibus quod aut et voluptate dolor non in tempora.
                Neque eum magnam iure hic quam quia.
                Omnis facere repellat minus perferendis suscipit voluptatem impedit qui harum.",
                                                "Sleek Soft Hat", "Et vel minus a quo molestias ratione.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 431, DateTimeKind.Local).AddTicks(
                                                    7140),
                                                "http://brown.biz", 1
                                            },
                                            {
                                                7,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 408, DateTimeKind.Local).AddTicks(
                                                    9338),
                                                @"Ipsa rerum eaque occaecati sequi ab cum similique voluptas doloremque.
                Alias voluptas consequatur ad.
                Quod voluptatem voluptatum repellat.
                Unde et asperiores est quisquam laudantium officia aut non ipsam.
                Repudiandae nihil nam fugit odit qui nostrum reprehenderit.
                Nobis aliquid illum ut qui doloribus non dignissimos perferendis.
                Consectetur et est accusantium dolores maiores ipsa fugiat et.
                Dicta nobis enim quo ducimus sunt laboriosam.
                Nesciunt dicta animi qui dicta sequi incidunt velit.
                Voluptatem perspiciatis qui magnam ratione et placeat consequuntur qui.",
                                                "Intelligent Cotton Chicken", "In dolorem magni veritatis.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 408, DateTimeKind.Local).AddTicks(
                                                    9407),
                                                "https://cordelia.name", 27
                                            },
                                            {
                                                20,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 465, DateTimeKind.Local).AddTicks(
                                                    5016),
                                                @"Aut autem necessitatibus ab aspernatur et enim.
                Rerum voluptatem veritatis dolorem iste libero sequi et.
                Ut ut accusantium quia aut qui sint et.
                Consequatur et omnis provident.
                Quaerat omnis numquam.
                Esse non officiis labore minima eveniet odio voluptate.
                In eaque dolor minima quis assumenda quisquam accusamus.
                Iusto reprehenderit et.
                Autem dolore aut consequatur vel et ex magnam.
                Et laboriosam reiciendis id similique autem aut doloribus magni.",
                                                "Intelligent Plastic Shoes",
                                                "Optio deleniti earum omnis iusto provident non doloribus.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 465, DateTimeKind.Local).AddTicks(
                                                    5080),
                                                "http://dominic.info", 25
                                            },
                                            {
                                                13,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 435, DateTimeKind.Local).AddTicks(
                                                    8720),
                                                @"Animi enim doloremque ut officiis rem.
                Debitis sequi alias est et dolore est.
                Distinctio optio qui culpa maiores mollitia placeat et.
                Velit aut aut suscipit distinctio non tempore saepe quidem.
                Esse veritatis incidunt quod at.
                Quae facere distinctio minus.
                Ut quaerat quam voluptate aut.
                Et minima autem dolor at.
                Nesciunt iste doloribus libero non numquam tempora sit porro.
                Necessitatibus nemo et et.",
                                                "Ergonomic Fresh Pants",
                                                "Mollitia quam earum nulla dicta optio occaecati dignissimos fuga eum.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 435, DateTimeKind.Local).AddTicks(
                                                    8799),
                                                "http://karlee.com", 25
                                            },
                                            {
                                                4,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 392, DateTimeKind.Local).AddTicks(
                                                    4304),
                                                @"Architecto laborum dolorem explicabo ratione quia ratione repellendus illum dolorem.
                Est libero similique.
                Perspiciatis fuga minus voluptates adipisci labore alias sit ut.
                Consequuntur placeat quo id.
                Necessitatibus harum aperiam ducimus amet minus et pariatur cum.
                Et commodi reprehenderit nihil eum repellat.
                Eos a laudantium voluptas consequatur consequatur nostrum et quasi.
                Natus et sit praesentium iusto.
                Inventore minima laudantium.
                Modi eum aut molestiae est inventore provident omnis.",
                                                "Awesome Plastic Shoes",
                                                "Est voluptates odio ipsum voluptas laborum possimus enim sint vitae.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 392, DateTimeKind.Local).AddTicks(
                                                    4377),
                                                "https://addison.info", 25
                                            },
                                            {
                                                3,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 387, DateTimeKind.Local).AddTicks(
                                                    4745),
                                                @"Asperiores laudantium quod.
                Cupiditate est omnis laborum perspiciatis.
                Praesentium aut error magnam tempora cum dolore sit recusandae.
                Voluptatum sed sapiente.
                Doloremque explicabo omnis quaerat ad quis provident perferendis aperiam maxime.
                Est non eaque mollitia aut exercitationem consequatur eum.
                Maiores error iste dolores.
                Eum non et consectetur earum animi blanditiis et maxime.
                Molestiae repudiandae occaecati suscipit.
                Nisi dignissimos earum qui officia.",
                                                "Ergonomic Wooden Chair",
                                                "Ratione non officia molestiae assumenda rem impedit.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 387, DateTimeKind.Local).AddTicks(
                                                    4833),
                                                "https://angel.info", 25
                                            },
                                            {
                                                16,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 448, DateTimeKind.Local).AddTicks(
                                                    8760),
                                                @"Culpa ducimus mollitia enim adipisci totam vitae voluptatem.
                Sint sit cum.
                Optio quia aut.
                Debitis modi soluta et numquam id ipsum dolore dolor beatae.
                Maxime dolorem voluptatem dolorem saepe sint explicabo et.
                Reiciendis quo reprehenderit dignissimos odit aut.
                Qui atque exercitationem eos corporis rerum consequuntur vel voluptas earum.
                Commodi quo nihil.
                Perferendis reprehenderit ducimus.
                Veniam consequatur pariatur dolor dicta.",
                                                "Handcrafted Rubber Soap", "Autem non hic eaque eum labore unde.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 448, DateTimeKind.Local).AddTicks(
                                                    8834),
                                                "http://macie.com", 23
                                            },
                                            {
                                                5,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 398, DateTimeKind.Local).AddTicks(
                                                    6481),
                                                @"Distinctio sint asperiores ipsam suscipit qui commodi quaerat culpa possimus.
                Ut et placeat quo non omnis praesentium consequatur sed.
                Eligendi pariatur soluta est eos velit adipisci asperiores nihil.
                Facilis et pariatur qui laborum.
                Natus ad voluptas architecto repudiandae maxime at eveniet minus.
                Sit et numquam sit distinctio dicta earum quia ut.
                Ipsam perspiciatis velit.
                Ipsum ut sequi quos non tempore eius ex modi voluptas.
                Amet animi quae quo.
                Asperiores dolor ipsa quis et voluptas omnis ea id natus.",
                                                "Rustic Cotton Car",
                                                "Tempore molestiae perspiciatis qui sit beatae illum et facilis officia.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 398, DateTimeKind.Local).AddTicks(
                                                    6574),
                                                "http://natasha.net", 22
                                            },
                                            {
                                                30,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 522, DateTimeKind.Local).AddTicks(
                                                    8997),
                                                @"Alias iste ut optio.
                Et minus quia.
                Repellendus magnam quo commodi ipsa ipsam quidem voluptas odio.
                Reprehenderit eius voluptas doloribus sint sunt debitis quis.
                Nisi blanditiis ipsam aut placeat sit.
                Dignissimos qui molestias dolores non voluptas distinctio quam vel.
                Nesciunt non fugiat voluptatem officiis et et nemo optio.
                Neque similique voluptatem quia labore aliquam nisi eum vel rem.
                Sapiente nisi non accusamus et.
                Nemo adipisci et natus quam.",
                                                "Sleek Steel Computer", "Nobis velit libero sed.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 522, DateTimeKind.Local).AddTicks(
                                                    9071),
                                                "https://johnpaul.net", 19
                                            },
                                            {
                                                26,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 495, DateTimeKind.Local).AddTicks(
                                                    9678),
                                                @"Dolor ea deserunt facilis omnis omnis totam molestiae et.
                Cum ea sunt nihil facere facilis exercitationem vel.
                Dolores est natus nihil.
                Quia nostrum velit sequi sed et totam molestiae assumenda omnis.
                Fugiat earum autem esse ratione ex laudantium dignissimos enim et.
                Vel at ducimus explicabo beatae consequuntur.
                Asperiores tempora dolorum reiciendis rerum cupiditate quaerat error.
                Est quidem dolorem dolore harum.
                Placeat sint cumque sint delectus.
                Dolor qui sed.",
                                                "Sleek Fresh Keyboard", "Dicta facilis voluptatum.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 495, DateTimeKind.Local).AddTicks(
                                                    9751),
                                                "http://myrtie.info", 18
                                            },
                                            {
                                                27,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 501, DateTimeKind.Local).AddTicks(
                                                    3622),
                                                @"Omnis suscipit at corporis at sequi culpa ipsa id.
                Qui dolorum nulla et aut provident reprehenderit neque neque.
                Non vitae quas et iste debitis repellendus.
                Est dolorum pariatur eum.
                At voluptas esse veritatis in veritatis.
                Deleniti id voluptate enim ut totam quibusdam reiciendis et.
                Quo dolorum quia eius.
                Totam eum maiores maxime aut quis ea.
                Sed a ipsam dolorem doloribus minus.
                Totam asperiores aspernatur aliquid aspernatur rem cum consectetur.",
                                                "Licensed Plastic Bacon",
                                                "Sunt beatae aut aut et et incidunt et ut quis.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 501, DateTimeKind.Local).AddTicks(
                                                    3724),
                                                "http://meaghan.com", 17
                                            },
                                            {
                                                24,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 486, DateTimeKind.Local).AddTicks(
                                                    6506),
                                                @"Distinctio eum est alias et earum eos.
                Nisi ea ut.
                Itaque fugit nemo laborum amet molestias.
                Nihil commodi sint labore dolorem aliquam.
                Veniam sit eum.
                Nihil totam aut at odit voluptatem enim consectetur.
                Recusandae tempore quia quisquam molestias corrupti suscipit dolorum id.
                Cupiditate molestiae harum quidem.
                Consectetur quod deserunt id placeat optio expedita.
                Tempore porro cumque quas enim dicta aut.",
                                                "Rustic Steel Chips",
                                                "Nisi repellat est blanditiis placeat ipsa adipisci qui cum sint.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 486, DateTimeKind.Local).AddTicks(
                                                    6589),
                                                "https://brigitte.com", 17
                                            },
                                            {
                                                25,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 491, DateTimeKind.Local).AddTicks(
                                                    5448),
                                                @"Iste nam consequatur qui nam odio nesciunt ad qui.
                Consequatur consequatur maiores veritatis provident mollitia quaerat dolor.
                Voluptatibus sit aut ratione perspiciatis eos molestiae eum numquam reiciendis.
                Vel perferendis et tempore porro nihil.
                Quaerat soluta iste corrupti dolor est reprehenderit consectetur.
                Est vero hic vitae fugiat soluta omnis sunt est placeat.
                Qui et id est rerum ullam cumque.
                Doloribus sit ipsam ut aut tenetur repellat et quis.
                Alias velit facere ex earum labore.
                Ut eveniet est dolores labore sed voluptatem delectus et qui.",
                                                "Refined Metal Car", "Earum rerum perspiciatis aut et iste.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 491, DateTimeKind.Local).AddTicks(
                                                    5512),
                                                "http://marie.com", 16
                                            },
                                            {
                                                6,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 404, DateTimeKind.Local).AddTicks(
                                                    1100),
                                                @"Et voluptatum corrupti saepe suscipit voluptatem.
                Provident et voluptates.
                Ea corrupti placeat.
                Est placeat perspiciatis molestiae soluta ipsam sed aut dolor eos.
                Ea et sunt voluptatibus.
                Necessitatibus iste voluptas id magni architecto doloremque suscipit.
                Totam sint sint dolorem id et.
                Quo voluptatum ipsa nemo.
                Ab non eos.
                Commodi nihil facere qui nam dignissimos dignissimos.",
                                                "Generic Frozen Mouse",
                                                "Magnam consectetur nulla neque ratione possimus in.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 404, DateTimeKind.Local).AddTicks(
                                                    1188),
                                                "http://mary.com", 15
                                            },
                                            {
                                                1,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 371, DateTimeKind.Local).AddTicks(
                                                    2908),
                                                @"Asperiores dolor necessitatibus neque dolore incidunt.
                Et et dolor rerum autem ipsa.
                Nisi voluptas assumenda optio.
                Autem inventore eligendi blanditiis ipsa qui ratione est.
                Amet molestias provident provident quo deleniti nulla sapiente.
                Quis distinctio doloremque at animi ullam quo.
                Velit animi adipisci iste vel ipsa et magni illo.
                Corporis est pariatur impedit aspernatur.
                Id debitis quia dolore velit distinctio error.
                Vel dolorem atque aperiam tempore.",
                                                "Incredible Fresh Pizza",
                                                "Praesentium reprehenderit dolorum quis ipsa perspiciatis dolores amet autem totam.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 375, DateTimeKind.Local).AddTicks(
                                                    5520),
                                                "https://bryce.net", 14
                                            },
                                            {
                                                2,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 381, DateTimeKind.Local).AddTicks(
                                                    3458),
                                                @"Culpa error corrupti sed quisquam dolor officia nisi.
                Eaque eveniet quia doloribus natus culpa blanditiis fugit.
                Repudiandae aut natus libero omnis quae unde quas et.
                Aut alias ea possimus eos.
                Et rem sit qui.
                Numquam aperiam rerum nostrum molestias animi aut eum.
                Suscipit rerum ut corporis harum dolores explicabo repudiandae aut dignissimos.
                Numquam cupiditate aliquam suscipit maxime harum eos.
                Qui expedita aut.
                Exercitationem sed optio quasi et.",
                                                "Rustic Granite Bike", "Quae ducimus ut sed placeat.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 381, DateTimeKind.Local).AddTicks(
                                                    4191),
                                                "https://gudrun.name", 13
                                            },
                                            {
                                                19,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 461, DateTimeKind.Local).AddTicks(
                                                    2694),
                                                @"Qui dolor beatae quod dolorum saepe.
                Consequatur et iusto aut.
                Et rerum maiores ad.
                Est aspernatur qui aliquam sint fuga.
                Et officia aut eveniet blanditiis.
                Nihil voluptate sint quae porro et autem dolore.
                Aut id laborum ab quis quis ea voluptatem.
                Soluta dolorem ut consectetur qui consequuntur sequi.
                Iste possimus itaque doloremque sint veniam unde modi.
                Nihil quia qui et suscipit eveniet quia et.",
                                                "Unbranded Granite Chicken", "Voluptatibus rerum et laboriosam rerum.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 461, DateTimeKind.Local).AddTicks(
                                                    2738),
                                                "https://ian.net", 12
                                            },
                                            {
                                                18,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 457, DateTimeKind.Local).AddTicks(
                                                    6013),
                                                @"Praesentium tempore qui.
                Reprehenderit dolorem fugit ullam non ad consequatur illo veritatis non.
                Amet cumque minus assumenda ex laboriosam perferendis.
                Temporibus provident occaecati veniam est dolores labore quas.
                Corrupti dolorem eos repudiandae.
                Architecto minima totam cupiditate aut.
                Qui sunt placeat.
                Enim ab aut facere voluptatem earum illum qui dignissimos alias.
                Consequatur officiis rerum nisi aut quis est velit error.
                Reprehenderit et ut fuga qui ullam fugiat blanditiis maiores ad.",
                                                "Small Metal Mouse",
                                                "Repellendus dolorum ut quisquam atque corrupti amet tempora.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 457, DateTimeKind.Local).AddTicks(
                                                    6066),
                                                "https://colby.info", 11
                                            },
                                            {
                                                28,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 507, DateTimeKind.Local).AddTicks(
                                                    2230),
                                                @"Corporis quis ex.
                Ullam aut et natus nihil quisquam occaecati dolorem.
                Sunt autem quidem.
                Et saepe error.
                Alias non est distinctio non nihil.
                Aspernatur reprehenderit laboriosam aut voluptates voluptatibus.
                Iure quo et temporibus.
                Natus velit qui amet impedit.
                Fugit adipisci quas hic maiores alias consectetur debitis qui.
                Beatae et asperiores consequatur aut.",
                                                "Handcrafted Steel Table",
                                                "Laudantium enim vel molestias repudiandae eos et voluptate et.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 507, DateTimeKind.Local).AddTicks(
                                                    2308),
                                                "http://kyler.com", 9
                                            },
                                            {
                                                21,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 471, DateTimeKind.Local).AddTicks(
                                                    3683),
                                                @"Omnis fuga ex rem dicta in ab esse assumenda.
                Odio ullam unde velit non omnis aperiam corporis quo quia.
                Iusto corrupti dignissimos ad.
                Officiis perspiciatis ab similique id sint adipisci.
                Sit dolorem facere sint ipsa odit est et ipsum repellendus.
                Ut nam temporibus aut iusto sed voluptatem aut in mollitia.
                Eos repudiandae adipisci.
                Cumque molestias commodi sed ut et.
                In sit omnis.
                Sint voluptatem est.",
                                                "Incredible Fresh Cheese", "Aut ut cumque ducimus ipsa itaque vitae.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 471, DateTimeKind.Local).AddTicks(
                                                    3771),
                                                "https://clinton.biz", 9
                                            },
                                            {
                                                17,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 453, DateTimeKind.Local).AddTicks(
                                                    6656),
                                                @"Odit optio laboriosam quo hic.
                Minus dicta inventore tenetur qui alias asperiores fugiat illum.
                Animi cumque non ut molestiae aut dolor.
                Et nihil sint aliquam delectus nihil quaerat magni qui.
                Ipsum qui accusantium.
                Fugiat ut accusamus.
                Rem asperiores eius eum et.
                Exercitationem consequuntur sed inventore cum in voluptate asperiores.
                Similique laboriosam dolore suscipit deleniti quia ex.
                Eligendi omnis debitis distinctio incidunt mollitia officia accusamus.",
                                                "Handmade Fresh Sausages",
                                                "Libero quidem rerum sit omnis libero praesentium et a voluptas.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 453, DateTimeKind.Local).AddTicks(
                                                    6739),
                                                "http://neha.net", 8
                                            },
                                            {
                                                10,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 422, DateTimeKind.Local).AddTicks(
                                                    1783),
                                                @"Voluptatem in iste eius magnam laboriosam beatae quo et.
                Cupiditate harum omnis distinctio quia voluptas enim debitis.
                Qui minima aut debitis quia rerum molestiae aut cum.
                Deleniti deleniti rerum minima facere aut repellendus inventore.
                Ipsa est dolor facere earum sunt molestias.
                Ullam impedit animi dolorum enim fugiat non consequuntur corrupti.
                Cumque incidunt sit voluptas ut.
                Illo omnis quia rerum ex voluptatem.
                Suscipit dolor aut quo ratione voluptate non et.
                Vel magni et qui beatae ipsa non est a.",
                                                "Handmade Soft Fish", "Voluptatem facere dicta eum.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 422, DateTimeKind.Local).AddTicks(
                                                    1856),
                                                "https://jake.name", 8
                                            },
                                            {
                                                23,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 481, DateTimeKind.Local).AddTicks(
                                                    9327),
                                                @"Laborum necessitatibus enim ut qui temporibus voluptatem.
                Esse dolores odio dolorem quia qui minima a.
                Sequi non amet et.
                Consequatur temporibus dolores corporis nostrum recusandae ullam.
                Voluptatibus in aut laudantium rerum maxime in dicta.
                Totam rem nisi rerum.
                Et eum dolorem voluptatem a maxime.
                Voluptatem et ut distinctio consequatur aut occaecati quod voluptas dolores.
                Voluptate assumenda voluptatem.
                Adipisci distinctio officiis.",
                                                "Incredible Rubber Keyboard",
                                                "Vel et repudiandae praesentium porro natus quisquam.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 481, DateTimeKind.Local).AddTicks(
                                                    9401),
                                                "http://mayra.com", 4
                                            },
                                            {
                                                9,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 418, DateTimeKind.Local).AddTicks(
                                                    643),
                                                @"Ab soluta numquam est exercitationem quis omnis.
                Ipsa nobis architecto molestiae unde eaque minus.
                Cupiditate tempora ut debitis id corporis corrupti.
                Magnam sit quis corporis quia sint dolorum.
                Quod natus sint blanditiis velit voluptates et dolore vel voluptatum.
                Corporis pariatur numquam fuga ullam recusandae atque blanditiis eaque.
                Itaque nulla maxime consectetur.
                Ex minima repellat rem in quia veritatis.
                Est doloribus cupiditate ad.
                Laborum qui nostrum et quaerat.",
                                                "Gorgeous Metal Bacon",
                                                "Sit maxime aperiam reprehenderit architecto consequatur.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 418, DateTimeKind.Local).AddTicks(
                                                    716),
                                                "https://amani.org", 4
                                            },
                                            {
                                                8,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 413, DateTimeKind.Local).AddTicks(
                                                    5655),
                                                @"Cupiditate illo est.
                Veniam molestiae dolor praesentium quo sed quibusdam.
                Voluptatem sit quis cumque suscipit.
                Illum natus officia et et earum sed doloremque accusantium nostrum.
                Veniam qui incidunt nesciunt minus et corrupti vitae maxime.
                Earum sunt nam voluptatibus est id accusamus voluptatibus ipsa.
                Nesciunt totam illum.
                Voluptatum sint saepe consectetur.
                Ut aliquam ut necessitatibus deserunt iste eos error.
                Esse occaecati quibusdam dolorum.",
                                                "Handmade Frozen Chair",
                                                "Vitae debitis doloribus et facilis quia suscipit.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 413, DateTimeKind.Local).AddTicks(
                                                    5743),
                                                "http://sabina.net", 3
                                            },
                                            {
                                                29,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 516, DateTimeKind.Local).AddTicks(
                                                    9817),
                                                @"Sit eum illum.
                Recusandae voluptatem voluptate laboriosam illum aperiam magnam.
                Enim non dolore nihil ipsum facere hic esse aut.
                Repudiandae reiciendis ad aspernatur.
                Eos dolores similique debitis.
                Doloremque illo est dolorem omnis mollitia debitis ipsam et.
                Est aut rerum.
                Explicabo deserunt animi.
                Qui dolore voluptatem vero eius qui.
                Voluptas ut fugiat alias ut sed magnam sequi dolores quo.",
                                                "Small Frozen Ball", "Animi vitae accusamus sunt debitis.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 516, DateTimeKind.Local).AddTicks(
                                                    9915),
                                                "https://salvatore.name", 2
                                            },
                                            {
                                                22,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 476, DateTimeKind.Local).AddTicks(
                                                    6004),
                                                @"Eveniet doloremque voluptas quia omnis ea.
                Nihil delectus unde quasi.
                Dicta est sed molestiae.
                Omnis quas perferendis quos pariatur odio laudantium voluptatem.
                Et pariatur maxime quo occaecati sint quos.
                Veniam minus dolorem adipisci.
                Sed nihil in ullam voluptatem autem sint pariatur.
                Ut amet soluta tenetur et animi.
                Quae deserunt vel ipsum alias temporibus nostrum a magni doloremque.
                Quidem necessitatibus quia iure quaerat quos ab.",
                                                "Handmade Cotton Soap", "Deserunt explicabo omnis quis dicta.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 476, DateTimeKind.Local).AddTicks(
                                                    6077),
                                                "https://letitia.info", 2
                                            },
                                            {
                                                14,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 440, DateTimeKind.Local).AddTicks(
                                                    2784),
                                                @"Error modi architecto sit eos quos quo.
                Et ea quidem quod consequatur aut autem.
                Eum inventore vero et itaque voluptas.
                Rerum aut consequatur veniam.
                Iusto rerum enim aut delectus.
                Maxime reprehenderit ex est minus.
                Odit nemo expedita exercitationem aut nihil aliquam enim eos voluptas.
                A possimus eum quidem.
                Vitae et architecto quos quia iste aut autem sunt.
                Est ab consequatur in aspernatur est quia.",
                                                "Small Rubber Ball",
                                                "Voluptas harum odit nulla possimus ea fugit perferendis eos aut.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 440, DateTimeKind.Local).AddTicks(
                                                    2857),
                                                "https://orlo.name", 1
                                            },
                                            {
                                                15,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 443, DateTimeKind.Local).AddTicks(
                                                    9999),
                                                @"Et cumque labore quisquam unde.
                Et quia ut non.
                Ut quod tempora mollitia.
                Eum et consequatur eligendi ut cum libero vel.
                Quam doloribus ex id.
                Quidem quis nihil dicta.
                Voluptates vitae quia dolor.
                Et rerum voluptate voluptatum voluptates aut.
                Aut quo porro sint quidem cum.
                Beatae dolore doloremque eos.",
                                                "Rustic Metal Sausages",
                                                "Consequuntur non molestias totam nesciunt exercitationem fugiat.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 444, DateTimeKind.Local).AddTicks(
                                                    53),
                                                "http://ferne.info", 27
                                            },
                                            {
                                                11,
                                                new DateTime(2020, 3, 27, 19, 1, 33, 427, DateTimeKind.Local).AddTicks(
                                                    951),
                                                @"Accusamus in ad illo voluptatem quo laudantium asperiores repellendus quisquam.
                Amet voluptates esse sed optio voluptas quia perferendis unde non.
                Quia possimus eveniet magnam tenetur et quia expedita.
                Eum rerum officiis rerum qui aliquid laboriosam illum.
                Placeat et ipsum molestiae.
                Error quia velit ut sit.
                Blanditiis est harum reprehenderit amet adipisci sint nesciunt.
                Aut fugiat officiis.
                Explicabo dolores corporis eaque.
                Rem ut et quas consequatur et.",
                                                "Licensed Metal Mouse",
                                                "Excepturi dolor blanditiis suscipit architecto perspiciatis expedita odit consequatur.",
                                                new DateTime(2020, 3, 29, 19, 1, 33, 427, DateTimeKind.Local).AddTicks(
                                                    1000),
                                                "http://micheal.info", 29
                                            }
                                        });

            migrationBuilder.InsertData("Collaborators",
                                        new[] {"Id", "FullName", "ProjectId", "Role"},
                                        new object[,]
                                        {
                                            {9385, "Fiona Gaylord", 12, "Internal Data Architect"},
                                            {2985, "Damion Daniel", 25, "Investor Integration Developer"},
                                            {6648, "Elinore Torphy", 25, "Human Usability Producer"},
                                            {9784, "Tavares Weissnat", 24, "Internal Division Associate"},
                                            {385, "Katherine Walsh", 24, "International Optimization Architect"},
                                            {5713, "Lavina Ward", 27, "Legacy Applications Administrator"},
                                            {6474, "Burnice Zieme", 27, "Future Intranet Assistant"},
                                            {7166, "Onie Leuschke", 26, "Lead Communications Executive"},
                                            {100, "Henriette Metz", 26, "Regional Factors Supervisor"},
                                            {7596, "Susana Skiles", 30, "Central Integration Assistant"},
                                            {7406, "Jaylen Doyle", 30, "International Security Assistant"},
                                            {7301, "Geovanni Sipes", 5, "Dynamic Integration Consultant"},
                                            {9127, "Lexus Kutch", 5, "Chief Optimization Architect"},
                                            {3251, "Keaton Considine", 6, "Central Implementation Executive"},
                                            {7142, "Rafael Gleason", 16, "Customer Interactions Administrator"},
                                            {7567, "Bertrand Bernhard", 3, "District Mobility Coordinator"},
                                            {3386, "Melyna Marks", 3, "International Response Coordinator"},
                                            {8461, "Damien Schinner", 4, "Chief Tactics Designer"},
                                            {9948, "Coby Wilderman", 4, "District Factors Manager"},
                                            {4959, "Brennon McClure", 13, "Customer Brand Assistant"},
                                            {8750, "Alicia McKenzie", 13, "Customer Security Designer"},
                                            {5548, "Erling Nolan", 20, "Principal Marketing Developer"},
                                            {1298, "Augustine Cartwright", 20, "Investor Brand Architect"},
                                            {6689, "Eddie Lindgren", 7, "Senior Division Consultant"},
                                            {6337, "Everett Dickinson", 7, "Internal Assurance Director"},
                                            {8942, "Hudson Hand", 15, "District Markets Associate"},
                                            {6804, "Linnea Baumbach", 15, "Forward Metrics Architect"},
                                            {3556, "Flo Gerlach", 16, "International Integration Manager"},
                                            {9835, "Kory Ward", 6, "Central Identity Technician"},
                                            {185, "Marcia Terry", 1, "International Markets Agent"},
                                            {678, "Jana Lesch", 1, "Future Intranet Associate"},
                                            {1678, "Rose Marks", 12, "Customer Optimization Strategist"},
                                            {6074, "Kieran Olson", 14, "Human Tactics Manager"},
                                            {5757, "Jamel Herman", 14, "Dynamic Data Strategist"},
                                            {4996, "Melyna Johns", 22, "Human Factors Analyst"},
                                            {4178, "Geraldine Schoen", 22, "Global Usability Planner"},
                                            {5101, "Alexie Weimann", 29, "Internal Branding Associate"},
                                            {8446, "Stephanie Grimes", 29, "Corporate Program Orchestrator"},
                                            {5598, "Blaise Kulas", 8, "Legacy Data Technician"},
                                            {8630, "Sophie Renner", 8, "National Factors Producer"},
                                            {6098, "Johnpaul Simonis", 9, "Product Security Coordinator"},
                                            {621, "Diana Dickens", 9, "International Infrastructure Representative"},
                                            {8288, "Jeremie Pagac", 23, "Human Brand Executive"},
                                            {3963, "Emely Wiegand", 23, "Senior Research Supervisor"},
                                            {2592, "Rhea Crooks", 10, "Legacy Paradigm Liaison"},
                                            {2677, "Bonnie Swaniawski", 10, "Customer Factors Engineer"},
                                            {7848, "Dora Lebsack", 17, "National Metrics Associate"},
                                            {6246, "Duane Heaney", 17, "Investor Quality Orchestrator"},
                                            {8031, "Rosalind Klocko", 21, "Forward Integration Coordinator"},
                                            {6428, "Laurie Bosco", 21, "Lead Markets Representative"},
                                            {9131, "Patricia Boyer", 28, "Future Group Assistant"},
                                            {1311, "Austen Will", 28, "Dynamic Security Developer"},
                                            {4816, "Isaiah Nicolas", 18, "Global Interactions Architect"},
                                            {4907, "Richie McCullough", 18, "Dynamic Directives Analyst"},
                                            {9501, "Alana Lockman", 19, "Chief Configuration Administrator"},
                                            {7016, "Chad Effertz", 19, "Direct Web Liaison"},
                                            {2754, "Helen Simonis", 2, "Lead Tactics Architect"},
                                            {1950, "Sandra Muller", 2, "District Usability Planner"},
                                            {1821, "Kayden Romaguera", 11, "Central Communications Specialist"},
                                            {3890, "Estella Herzog", 11, "Forward Applications Representative"}
                                        });

            migrationBuilder.CreateIndex("IX_Collaborators_ProjectId",
                                         "Collaborators",
                                         "ProjectId");

            migrationBuilder.CreateIndex("IX_Project_UserId",
                                         "Project",
                                         "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Collaborators");

            migrationBuilder.DropTable("Project");

            migrationBuilder.DropTable("User");
        }

    }

}
