using System;
using System.IO;

using Xunit;

using ReallyBad.IO;

namespace ReallyBad.IO.Test
{

    public class FileOrganizerTest
    {

        [Fact]
        public void IsDateFormatDirectoryTest()
        {
            Assert.True( FileOrganizer.IsDateFormatDirectory( "2018-12-31", "yyyy-MM-dd" ) );
            Assert.False( FileOrganizer.IsDateFormatDirectory( "201-12-31", "yyyy-MM-dd" ) );
            Assert.False( FileOrganizer.IsDateFormatDirectory( "201x-12-31", "yyyy-MM-dd" ) );
            FileOrganizer fileOrganizer = new FileOrganizer();
            Assert.True( fileOrganizer.IsDateFormatDirectory( "2018-12-31" ) );
            Assert.False( fileOrganizer.IsDateFormatDirectory( "201-12-31" ) );
            Assert.False( fileOrganizer.IsDateFormatDirectory( "201x-12-31" ) );
        }


        [Fact]
        public void GetRelativePathTest()
        {
            string root = @"C:\Root";
            string sub = @"Sub1\Sub2";
            string subFolder = root + @"\" + sub;
            string relative = FileOrganizer.GetRelativePath( subFolder, root );
            Assert.Equal( sub, relative );
        }

        [Fact]
        public void GetDestinationPathTest()
        {
            //string source = @"C:\Source";
            string dest = @"C:\Dest";
            string formatted = "2018-12-31";
            string fileText = "file.txt";
            string backslash = @"\";
            string expected = dest + backslash + formatted + backslash + fileText;
            string destinationPath = FileOrganizer.GetDestinationPath( fileText, dest, formatted );
            Assert.Equal( expected, destinationPath );
        }

        [Fact]
        public void GetDestinationParentDirectoryTest()
        {

            string backslash = @"\";
            string source = @"C:\Source";
            string dest = @"C:\Dest";
            string fileText = "file.txt";
            string file = source + backslash + fileText;

            FileInfo fileInfo = new FileInfo( file );

            FileOrganizer fileOrganizer = new FileOrganizer
            {
                SourceRootDirectory = source,
                DestRootDirectory = dest
            };

            string destParent = fileOrganizer.GetDestinationParentDirectory( fileInfo );

            Assert.Equal( dest, destParent );

            // test one level sub

            string sub = backslash + "Sub";
            string sourceSub = source + sub;
            string destSub = dest + sub;
            string fileSub = sourceSub + backslash + fileText;

            FileInfo fileInfoSub = new FileInfo( fileSub );

            FileOrganizer fileOrganizerSub = new FileOrganizer()
            {
                SourceRootDirectory = source,
                DestRootDirectory = dest,
                IsPreserveTree = true
            };

            string destSubParent = fileOrganizerSub.GetDestinationParentDirectory( fileInfoSub );

            Assert.Equal( destSub, destSubParent );

            // test 2 level sub

            string sourceSub2 = source + sub + "1" + sub + "2";
            string destSub2 = dest + sub + "1" + sub + "2";
            string fileSub2 = sourceSub2 + backslash + fileText;

            FileInfo fileInfoSub2 = new FileInfo( fileSub2 );

            FileOrganizer fileOrganizerSub2 = new FileOrganizer()
            {
                SourceRootDirectory = source,
                DestRootDirectory = dest,
                IsPreserveTree = true
            };

            string destSubParent2 = fileOrganizerSub2.GetDestinationParentDirectory( fileInfoSub2 );

            Assert.Equal( destSub2, destSubParent2 );

            // test 2 level in formatted directory

            string sourceSub3 = source + sub + "1" + sub + "2" + backslash + "2019-01-01";
            string destSub3 = dest + sub + "1" + sub + "2";
            string fileSub3 = sourceSub3 + backslash + fileText;

            FileInfo fileInfoSub3 = new FileInfo( fileSub3 );

            FileOrganizer fileOrganizerSub3 = new FileOrganizer()
            {
                SourceRootDirectory = source,
                DestRootDirectory = dest,
                IsPreserveTree = true,
                ImageFileInfoProvider = new MockImageFileInfoProvider()
            };

            string destSubParent3 = fileOrganizerSub3.GetDestinationParentDirectory( fileInfoSub3 );

            Assert.Equal( destSub3, destSubParent3 );

        }

    }

}
