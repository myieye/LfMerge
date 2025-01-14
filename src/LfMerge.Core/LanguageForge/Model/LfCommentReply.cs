﻿// Copyright (c) 2016 SIL International
// This software is licensed under the MIT license (http://opensource.org/licenses/MIT)
using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LfMerge.Core.LanguageForge.Model
{
	[BsonIgnoreExtraElements] // WARNING: Beware of using FindOneAndReplace() with IgnoreExtraElements, as you can lose data
	public class LfCommentReply : LfFieldBase, IHasNullableGuid
	{
		[BsonRepresentation(BsonType.String)]
		public Guid? Guid { get; set; }
		public LfAuthorInfo AuthorInfo { get; set; }
		public string AuthorNameAlternate { get; set; } // Used in sending comments to FW; should be null when serializing to Mongo
		public string Content { get; set; }
		[BsonElement("id")]
		public string UniqId { get; set; }  // If we name this field "Id", the C# driver tries to map it to _id and always thinks it is null
		public bool IsDeleted { get; set; }

		public bool ShouldSerializeGuid() { return (Guid != null && Guid.Value != System.Guid.Empty); }
		public bool ShouldSerializeContent() { return ( ! String.IsNullOrEmpty(Content)); }
		public bool ShouldSerializeAuthorNameAlternate() { return ( ! String.IsNullOrEmpty(AuthorNameAlternate)); }
		public bool ShouldSerializeId() { return ( ! String.IsNullOrEmpty(UniqId)); }
		// We almost always want to store the IsDeleted value, unless the reply is pretty much empty of any useful content.
		public bool ShouldSerializeIsDeleted() { return IsDeleted || ShouldSerializeGuid() || ShouldSerializeContent() || ShouldSerializeId(); }

		public LfCommentReply() {
			// Any lists, etc., should be populated in here
		}
	}
}

