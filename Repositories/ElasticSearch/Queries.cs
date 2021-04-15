using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.ElasticSearch
{
    public class Queries
    {
        private string projectRecommendations = "{ \"query\":{ \"bool\":{ \"must\":{ \"term\":{ \"Likes\":\"ReplaceWithSimilarUserId\" } }," +
            " \"must_not\":{ \"term\":{ \"Likes\":\"ReplaceWithUserId\" } } } } }";
        private string similarUsers = "{ \"aggs\":{ \"user-liked\":{ \"filter\":{ \"term\":{ \"Likes\":\"ReplaceWithUserId\" } }, " +
            "\"aggs\":{ \"bucket\":{ \"terms\":{ \"field\":\"Likes\" } } } } } }";
        private string indexProjects = "{ \"settings\":{ \"analysis\":{ \"analyzer\":{ \"autocomplete\":{ \"tokenizer\":\"autocomplete\", " +
            "\"filter\":[\"lowercase\"]},\"autocomplete_search\":" +
            "{\"tokenizer\":\"lowercase\"},\"description_index\":{ \"tokenizer\":\"lowercase\"," +
            "\"filter\":[\"synonym\",\"english_stop\",\"english_stemmer\"]},\"description_search\":" +
            "{ \"tokenizer\":\"lowercase\"}},\"filter\":{ \"synonym\":{ \"type\":\"synonym\",\"format\":" +
            "\"wordnet\",\"lenient\":true,\"synonyms_path\":\"analysis/wn_s.txt\"},\"english_stop\":" +
            "{ \"type\":\"stop\",\"stopwords\":\"_english_\"},\"english_stemmer\":{ \"type\":\"stemmer\"," +
            "\"language\":\"english\"} },\"tokenizer\":{ \"autocomplete\":{ \"type\":\"edge_ngram\"," +
            "\"min_gram\":2,\"max_gram\":10,\"token_chars\":[\"letter\"]} }}},\"mappings\":{ \"properties\":" +
            "{ \"Created\":{ \"type\":\"date\"},\"Id\":{ \"type\":\"integer\"},\"ProjectName\":{ \"type\":" +
            "\"text\",\"analyzer\":\"autocomplete\",\"search_analyzer\":\"autocomplete_search\"},\"Description\":" +
            "{ \"type\":\"text\",\"analyzer\":\"description_index\",\"search_analyzer\":\"description_search\"}," +
            "\"Likes\":{ \"type\":\"integer\"} } }}";

        public string ProjectRecommendations { get => projectRecommendations; }
        public string SimilarUsers { get => similarUsers; }
        public string IndexProjects { get => indexProjects; }
    }
}
