using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Tests.DataGenerators
{
    /// <summary>
    ///     This class is used for test purposes to return expected results that normally the ElasticSearch cluster would return.
    /// </summary>
    public class ElasticSearchResults
    {
        /// <summary>
        ///     The result that the ElasticSearch would normally return after the query for getting LikedProjectsFromSimilarUsers
        /// </summary>
        static public string GetLikedProjectsFromSimilarUserResult = "{\r\n\"took\":4,\r\n\"timed_out\":false,\r\n\"_shards\":{\r\n\"total\":1,\r\n\"successful\":1,\r\n\"skipped\":0,\r\n\"failed\":0\r\n},\r\n\"hits\":{\r\n\"total\":{\r\n\"value\":5,\r\n\"relation\":\"eq\"\r\n},\r\n\"max_score\":1.0,\r\n\"hits\":[\r\n{\r\n\"_index\":\"projects\",\r\n\"_type\":\"_doc\",\r\n\"_id\":\"9\",\r\n\"_score\":1.0,\r\n\"_source\":{\r\n\"Created\":\"2021-03-13T14:37:28.5859153\",\r\n\"Id\":9,\r\n\"ProjectName\":\"IntelligentMetalChair\",\r\n\"Description\":\"Doloresperferendisdignissimosearum.\\nOccaecatieaconsequaturnihilincidunt.\\nUtnumquamhic.\\nNostrumexercitationemetvelitnisidoloremquefacilisveritatismaioresex.\\nQuamnobiseacorporissitaspernaturvelmolestiaesuntporro.\\nEosnemomolestias.\\nOditvelundevoluptatemutvoluptatequodvoluptatumsintdelectus.\\nAtqueestmagnicorruptihicblanditiisofficia.\\nAutquisquamsapientequiaabet.\\nTemporibusautemquiatempora.\",\r\n\"Likes\":[\r\n1,\r\n21,\r\n9,\r\n32,\r\n6,\r\n30,\r\n10,\r\n13,\r\n3,\r\n16,\r\n33\r\n]\r\n}\r\n},\r\n{\r\n\"_index\":\"projects\",\r\n\"_type\":\"_doc\",\r\n\"_id\":\"12\",\r\n\"_score\":1.0,\r\n\"_source\":{\r\n\"Created\":\"2021-03-13T14:37:28.5989939\",\r\n\"Id\":12,\r\n\"ProjectName\":\"FantasticFreshComputer\",\r\n\"Description\":\"Perferendisliberoabnon.\\nQuiacumquevoluptatibusevenietsintsimiliqueetquidolor.\\nTemporaideligenditeneturquisrerum.\\nExplicaboconsequaturfugitrationeaperiam.\\nEssefacereautsequiaperiam.\\nVelitetabconsequuntursitpraesentiumexplicaboautest.\\nIninquisquamsuscipitquasiautinimpeditet.\\nVeniamidsuntlaboreillumfacilisquiacommodi.\\nRationeimpeditveliteumestquisqui.\\nVoluptatumcumdoloresminima.\",\r\n\"Likes\":[\r\n1,\r\n6,\r\n29,\r\n7,\r\n20,\r\n23,\r\n27,\r\n18,\r\n14,\r\n19,\r\n17,\r\n28,\r\n8\r\n]\r\n}\r\n},\r\n{\r\n\"_index\":\"projects\",\r\n\"_type\":\"_doc\",\r\n\"_id\":\"19\",\r\n\"_score\":1.0,\r\n\"_source\":{\r\n\"Created\":\"2021-03-13T14:37:28.6265708\",\r\n\"Id\":19,\r\n\"ProjectName\":\"GorgeousRubberHat\",\r\n\"Description\":\"Natusblanditiisautofficiaidiurenihil.\\nQuiaquisveniammagnamvelitblanditiisuteveniet.\\nExcepturieumcupiditateexquiquaeautquiautinventore.\\nEaarchitectoassumendaexercitationemnostrumnamaut.\\nQuoeaquemagniexcepturisaepehicunde.\\nEtestauteminventoreincommodiaspernaturmaximeeiusasperiores.\\nNequevoluptatecorporisesse.\\nVoluptaspariaturexercitationemabsintdoloribussapientemaximedeleniti.\\nInventoreveroeumofficiisharumeapariaturnesciunt.\\nDucimusnonalias.\",\r\n\"Likes\":[\r\n1,\r\n5,\r\n25,\r\n13,\r\n21,\r\n19,\r\n7,\r\n23,\r\n29,\r\n27,\r\n8,\r\n30,\r\n6\r\n]\r\n}\r\n},\r\n{\r\n\"_index\":\"projects\",\r\n\"_type\":\"_doc\",\r\n\"_id\":\"21\",\r\n\"_score\":1.0,\r\n\"_source\":{\r\n\"Created\":\"2021-03-13T14:37:28.635113\",\r\n\"Id\":21,\r\n\"ProjectName\":\"PracticalWoodenBall\",\r\n\"Description\":\"Etquiasintlaboriosamquoamet.\\nVoluptatumquiaestnostrumculpaaspernatureummodi.\\nEtrepudiandaenostrumquiexcepturi.\\nQuisaquieumquasinesciuntomnis.\\nPraesentiumquiaquisofficiahicnequenecessitatibus.\\nEaqueporroconsequatur.\\nMinusincorruptimolestiaedolordoloremomnisetipsaperspiciatis.\\nOptionecessitatibusquietisteestquifugitsunt.\\nFacilisvoluptatemdoloremnonmolestiasautveritatisblanditiis.\\nOfficiisineiusmollitiaomnisimpeditremullamquia.\",\r\n\"Likes\":[\r\n1,\r\n18,\r\n33,\r\n8,\r\n22,\r\n30,\r\n6,\r\n29\r\n]\r\n}\r\n},\r\n{\r\n\"_index\":\"projects\",\r\n\"_type\":\"_doc\",\r\n\"_id\":\"27\",\r\n\"_score\":1.0,\r\n\"_source\":{\r\n\"Created\":\"2021-03-13T14:37:28.6850052\",\r\n\"Id\":27,\r\n\"ProjectName\":\"LicensedSteelSoap\",\r\n\"Description\":\"Corporiserrorillumvelitdoloresofficiis.\\nDelectusfacereautenimsitetsed.\\nEummolestiaenequeomnisharumdignissimosipsa.\\nFugiatauttotamrerumdelectusvelfacilis.\\nRemesseomnisnon.\\nIllumquiarerumaperiamsapiente.\\nNonestrem.\\nSuscipitsimiliqueblanditiisetnecessitatibusdistinctio.\\nAutavoluptatemdoloribus.\\nProvidentliberoinciduntculpasequiautrerumetsintnostrum.\",\r\n\"Likes\":[\r\n1,\r\n27,\r\n4,\r\n15,\r\n7,\r\n22\r\n]\r\n}\r\n}\r\n]\r\n}\r\n}";

        /// <summary>
        ///     The result that the ElasticSearch would normally return after the query for getting GetSimilarUserResult
        /// </summary>
        static public string GetSimilarUserResult = "{\r\n\"took\":23,\r\n\"timed_out\":false,\r\n\"_shards\":{\r\n\"total\":1,\r\n\"successful\":1,\r\n\"skipped\":0,\r\n\"failed\":0\r\n},\r\n\"hits\":{\r\n\"total\":{\r\n\"value\":30,\r\n\"relation\":\"eq\"\r\n},\r\n\"max_score\":null,\r\n\"hits\":[]\r\n},\r\n\"aggregations\":{\r\n\"user-liked\":{\r\n\"doc_count\":6,\r\n\"bucket\":{\r\n\"doc_count_error_upper_bound\":0,\r\n\"sum_other_doc_count\":28,\r\n\"buckets\":[\r\n{\r\n\"key\":1,\r\n\"doc_count\":6\r\n},\r\n{\r\n\"key\":6,\r\n\"doc_count\":4\r\n},\r\n{\r\n\"key\":7,\r\n\"doc_count\":4\r\n},\r\n{\r\n\"key\":30,\r\n\"doc_count\":4\r\n},\r\n{\r\n\"key\":8,\r\n\"doc_count\":3\r\n},\r\n{\r\n\"key\":23,\r\n\"doc_count\":3\r\n},\r\n{\r\n\"key\":27,\r\n\"doc_count\":3\r\n},\r\n{\r\n\"key\":29,\r\n\"doc_count\":3\r\n},\r\n{\r\n\"key\":33,\r\n\"doc_count\":3\r\n},\r\n{\r\n\"key\":10,\r\n\"doc_count\":2\r\n}\r\n]\r\n}\r\n}\r\n}\r\n}";

        /// <summary>
        ///     This is the query that needs to be sent to ElasticSearch for retreiving the LikedProjectsFromSimilarUser. Replace: "ReplaceWithSimilarUserId" with the id in question
        /// </summary>
        static public string GetLikedProjectsFromSimilarUserQuery = "{ \"query\":{ \"bool\":{ \"must\":{ \"term\":{ \"Likes\":\"ReplaceWithSimilarUserId\" } }," +
            " \"must_not\":{ \"term\":{ \"Likes\":\"ReplaceWithUserId\" } } } } }";

        /// <summary>
        ///     This is the query that needs to be sent to ElasticSearch for retreiving the SimilarUser. Replace: "ReplaceWithUserId" with the id in question
        /// </summary>
        static public string GetSimilarUserQuery = "{ \"aggs\":{ \"user-liked\":{ \"filter\":{ \"term\":{ \"Likes\":\"ReplaceWithUserId\" } }, " +
            "\"aggs\":{ \"bucket\":{ \"terms\":{ \"field\":\"Likes\" } } } } } }";

    }
}
