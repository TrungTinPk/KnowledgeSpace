﻿namespace JW.KS.ViewModels.Contents
{
    public class ReportCreateRequest
    {
        public int? KnowledgeBaseId { get; set; }

        public int? CommentId { get; set; }

        public string Content { get; set; }

        public string ReportUserId { get; set; }
    }
}