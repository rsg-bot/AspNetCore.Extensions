using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Rocket.Surgery.AspNetCore.Mvc.Linking
{
    public static class Relation
    {
        /// <summary>Designates a substitute for the link's context.</summary>
        public static string Alternate = "alternate";
        /// <summary>Refers to an appendix.</summary>
        public static string Appendix = "appendix";
        /// <summary>Refers to a bookmark or entry point.</summary>
        public static string Bookmark = "bookmark";
        /// <summary>Refers to a chapter in a collection of resources.</summary>
        public static string Chapter = "chapter";
        /// <summary>Refers to a table of contents.</summary>
        public static string Contents = "contents";
        /// <summary>Refers to a copyright statement that applies to the link's context.</summary>
        public static string Copyright = "copyright";
        /// <summary>Refers to a resource containing the most recent item(s) in a collection of resources.</summary>
        public static string Current = "current";
        /// <summary>Refers to a resource providing information about the link's context.</summary>
        public static string Describedby = "describedby";
        /// <summary>Refers to a resource that can be used to edit the link's context.</summary>
        public static string Edit = "edit";
        /// <summary>Refers to a resource that can be used to edit media associated with the link's context.</summary>
        public static string EditMedia = "edit-media";
        /// <summary>Identifies a related resource that is potentially large and might require special handling.</summary>
        public static string Enclosure = "enclosure";
        /// <summary>An IRI that refers to the furthest preceding resource in a series of resources.</summary>
        public static string First = "first";
        /// <summary>Refers to a glossary of terms.</summary>
        public static string Glossary = "glossary";
        /// <summary>Refers to a resource offering help (more information, links to other sources information, etc.)</summary>
        public static string Help = "help";
        /// <summary>Refers to a hub that enables registration for notification of updates to the context.</summary>
        public static string Hub = "hub";
        /// <summary>Refers to an index.</summary>
        public static string Index = "index";
        /// <summary>An IRI that refers to the furthest following resource in a series of resources.</summary>
        public static string Last = "last";
        /// <summary>Points to a resource containing the latest (e.g., current) version of the context.</summary>
        public static string LatestVersion = "latest-version";
        /// <summary>Refers to a license associated with the link's context.</summary>
        public static string License = "license";
        /// <summary>Refers to the next resource in a ordered series of resources.</summary>
        public static string Next = "next";
        /// <summary>Refers to the immediately following archive resource.</summary>        public static string next-archive = "next-archive";
        /// <summary>indicates a resource where payment is accepted.</summary>
        public static string Payment = "payment";
        /// <summary>Refers to the previous resource in an ordered series of resources.Synonym for "previous".</summary>
        public static string Prev = "prev";
        /// <summary>Points to a resource containing the predecessor version in the version history.</summary>
        public static string PredecessorVersion = "predecessor-version";
        /// <summary>Refers to the previous resource in an ordered series of resources.Synonym for "prev".</summary>
        public static string Previous = "previous";
        /// <summary>Refers to the immediately preceding archive resource.</summary>
        public static string PrevArchive = "prev-archive";
        /// <summary>Identifies a related resource.</summary>
        public static string Related = "related";
        /// <summary>Identifies a resource that is a reply to the context of the link.</summary>
        public static string Replies = "replies";
        /// <summary>Refers to a section in a collection of resources.</summary>
        public static string Section = "section";
        /// <summary>Conveys an identifier for the link's context.</summary>
        public static string Self = "self";
        /// <summary>Indicates a URI that can be used to retrieve a service document.</summary>
        public static string Service = "service";
        /// <summary>Refers to the first resource in a collection of resources.</summary>
        public static string Start = "start";
        /// <summary>Refers to an external style sheet.</summary>
        public static string Stylesheet = "stylesheet";
        /// <summary>Refers to a resource serving as a subsection in a collection of resources.</summary>
        public static string Subsection = "subsection";
        /// <summary>Points to a resource containing the successor version in the version history.</summary>
        public static string SuccessorVersion = "successor-version";
        /// <summary>Refers to a parent document in a hierarchy of documents.</summary>
        public static string Up = "up";
        /// <summary>points to a resource containing the version history for the context.</summary>
        public static string VersionHistory = "version-history";
        /// <summary>Identifies a resource that is the source of the information in the link's context.</summary>
        public static string Via = "via";
        /// <summary>Points to a working copy for this resource.</summary>
        public static string WorkingCopy = "working-copy";
        /// <summary>Points to the versioned resource from which this working copy was obtained.</summary>
        public static string WorkingCopyOf = "working-copy-of";
    }

    public class Link
    {
        public string Rel { get; }
        public string Href { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; }

        public Link(string relation, string href, string title = null)
        {
            Rel = relation ?? throw new ArgumentNullException(nameof(relation));
            Href = href ?? throw new ArgumentNullException(nameof(href));
            Title = title;
        }
    }

    public class LinkTemplate
    {
        public LinkTemplate(string rel, string href, string title = null)
        {
            Rel = rel ?? throw new ArgumentNullException(nameof(rel));
            Href = href ?? throw new ArgumentNullException(nameof(href));
            Title = title;
        }

        public string Rel { get; }
        public string Href { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; }
    }
}
