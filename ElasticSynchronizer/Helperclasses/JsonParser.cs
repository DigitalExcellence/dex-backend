using ElasticSynchronizer.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace ElasticSynchronizer.Helperclasses
{

    public interface IJsonParser
    {

        ESProjectDTO JsonStringToProjectES(string body);

    }

    public class JsonParser : IJsonParser
    {

        public ESProjectDTO JsonStringToProjectES(string body)
        {
            JToken token = JToken.Parse(body);
            ESProjectDTO project = new ESProjectDTO();
            project.Description = token.Value<string>("Description");
            project.ProjectName = token.Value<string>("Name");
            project.Id = token.Value<int>("Id");
            project.Created = token.Value<DateTime>("Created");
            project.Likes = token.Value<List<int>>("DescLikesription");

            return project;
        }

    }

}
