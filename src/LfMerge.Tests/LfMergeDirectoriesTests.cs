﻿// Copyright (c) 2011-2015 SIL International
// This software is licensed under the MIT license (http://opensource.org/licenses/MIT)
using System;
using System.IO;
using LfMerge;
using NUnit.Framework;
using Palaso.TestUtilities;
using LfMerge.Queues;

namespace LfMerge.Tests
{
	[TestFixture]
	public class LfMergeDirectoriesTests
	{
		[Test]
		public void FdoDirs_RelativePathsAreSubdirsOfBasedir()
		{
			var sut = new LfMergeDirectories(Path.GetTempPath(), "projects", "templates");

			Assert.That(sut.ProjectsDirectory, Is.EqualTo(Path.Combine(Path.GetTempPath(), "projects")));
			Assert.That(sut.DefaultProjectsDirectory, Is.EqualTo(Path.Combine(Path.GetTempPath(), "projects")));
			Assert.That(sut.TemplateDirectory, Is.EqualTo(Path.Combine(Path.GetTempPath(), "templates")));
		}

		[Test]
		public void FdoDirs_AbsolutePathsRemainAbsolute()
		{
			var sut = new LfMergeDirectories(Path.GetTempPath(), "/projects", "/foo/templates");

			Assert.That(sut.ProjectsDirectory, Is.EqualTo("/projects"));
			Assert.That(sut.DefaultProjectsDirectory, Is.EqualTo("/projects"));
			Assert.That(sut.TemplateDirectory, Is.EqualTo("/foo/templates"));
		}

		[Test]
		public void StateDirectory_Correct()
		{
			var sut = new LfMergeDirectories(Path.GetTempPath());
			Assert.That(sut.StateDirectory, Is.EqualTo(Path.Combine(Path.GetTempPath(), "state")));
		}

		[Test]
		public void GetStateFileName_Correct()
		{
			// Setup
			using (var temp = new TemporaryFolder("StateFile"))
			{
				var sut = new LfMergeDirectories(temp.Path);

				// Exercise
				var stateFile = sut.GetStateFileName("ProjA");

				// Verify
				Assert.That(stateFile, Is.EqualTo(Path.Combine(temp.Path, "state/ProjA.state")));
				Assert.That(Directory.Exists(Path.GetDirectoryName(stateFile)), Is.True,
					"State directory didn't get created");
			}
		}

		[TestCase(QueueNames.Commit, "commitqueue")]
		[TestCase(QueueNames.Merge, "mergequeue")]
		[TestCase(QueueNames.Receive, "receivequeue")]
		[TestCase(QueueNames.Send, "sendqueue")]
		[TestCase(QueueNames.None, null)]
		public void GetQueueDirectory_Correct(QueueNames queue, string expectedDir)
		{
			// Setup
			using (var temp = new TemporaryFolder("QueueDirectory"))
			{
				var sut = new LfMergeDirectories(temp.Path);

				// Exercise
				var queueDir = sut.GetQueueDirectory(queue);

				// Verify
				Assert.That(Path.GetFileName(queueDir), Is.EqualTo(expectedDir));
			}
		}
	}
}
