using Microsoft.AspNetCore.Razor.TagHelpers;
using StarCraft2League.Models.Users;

namespace StarCraft2League.TagHelpers
{
    public class ParticipantTagHelper : TagHelper
    {
        public User User { get; set; }

        public bool IsLink { get; set; } = true;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = IsLink ? "a" : "span";
            output.Attributes.SetAttribute("href", User.Profile.Url);
            output.Content.SetContent(User.DisplayedName);
            string leagueWithTier =
                User.Profile.League.Name + User.Profile.Tier;
            output.PreElement.SetHtmlContent(
                "<img src=\"/images/leagues/" +
                leagueWithTier +
                ".png\" title=\"" +
                leagueWithTier +
                "\"\"></img>"
                );
            output.PreElement.AppendHtml("<img src=\"/images/races/" + User.Profile.Race + ".png\"></img>");
        }
    }
}