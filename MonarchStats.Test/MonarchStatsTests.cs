using MonarchStatsConsoleApp;
using System;
using System.Collections.Generic;
using Xunit;

namespace MonarchStats.Test
{
    public class MonarchStatsTests
    {

        [Fact]
        public void GetTotalMonarchs_ShouldReturnCorrectCount()
        {
            // Arrange
            var monarchs = new List<Monarch>
        {
            new Monarch { Nm = "Elizabeth II", Yrs = "1952-2022" },
            new Monarch { Nm = "George VI", Yrs = "1936-1952" },
            new Monarch { Nm = "Edward VIII", Yrs = "1936-1936" }
        };

            // Expected number of monarchs
            int expectedCount = 3;

            // Act
            int totalCount = Program.GetTotalMonarchs(monarchs);

            // Assert
            Assert.Equal(expectedCount, totalCount);
        }


        [Fact]
        public void GetLongestReignMonarch_ShouldReturnCorrectResult()
        {
            // Arrange
            var monarchs = new List<Monarch>
        {
            new Monarch { Nm = "Henry VIII", Yrs = "1509-1547" },
            new Monarch { Nm = "Elizabeth II", Yrs = "1952-2022" }
        };

            // Act
            var result = Program.GetLongestReignMonarch(monarchs);

            // Assert
            Assert.Equal("Elizabeth II", result.Name);
            Assert.Equal(70, result.ReignLength);
        }

        [Fact]
        public void GetLongestRulingHouse_ShouldReturnCorrectResult()
        {
            // Arrange
            var monarchs = new List<Monarch>
                    {
                        new Monarch { Hse = "Tudor", Yrs = "1485-1603" },
                        new Monarch { Hse = "Windsor", Yrs = "1910-2022" }
                    };

            // Act
            var result = Program.GetLongestRulingHouse(monarchs);

            // Assert
            Assert.Equal("Tudor", result.House);
            Assert.Equal(118, result.TotalYears);
        }

        [Fact]
        public void GetMostCommonFirstName_ShouldReturnCorrectResult()
        {
            // Arrange
            var monarchs = new List<Monarch>
                {
                    new Monarch { Nm = "George I" },
                    new Monarch { Nm = "George II" },
                    new Monarch { Nm = "Elizabeth II" }
                };

            // Act
            var result = Program.GetMostCommonFirstName(monarchs);

            // Assert
            Assert.Equal("George", result);
        }


    }
}
