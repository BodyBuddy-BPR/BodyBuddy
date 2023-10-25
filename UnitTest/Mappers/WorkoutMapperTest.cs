using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Models;

namespace UnitTest.Mappers;

public class WorkoutMapperTest
{
    public WorkoutMapper target;

    [SetUp]
    public void SetUp()
    {
        target = new WorkoutMapper();
    }

    [Test]
    public void MapToDto_DoesNotReturnNull_WhenInputIsNull()
    {
        //Act
        var returnWorkoutDto = target.MapToDto(null);

        //Assert
        Assert.NotNull(returnWorkoutDto);
    }

    [TestCase(0, false)]
    [TestCase(1, true)]
    [TestCase(5, true)]
    [TestCase(15, true)]
    [TestCase(-1, true)]
    public void MapToDto_MapsCorrectBool(int preMade, bool expectedBool)
    {
        WorkoutModel workoutModel = new() { Id = 1, Description = "Description", Name = "Name", PreMade = preMade };

        //Act
        var returnWorkoutDto = target.MapToDto(workoutModel);

        //Assert
        Assert.That(returnWorkoutDto.Id, Is.EqualTo(1));
        Assert.That(returnWorkoutDto.Description, Is.EqualTo("Description"));
        Assert.That(returnWorkoutDto.Name, Is.EqualTo("Name"));
        Assert.That(returnWorkoutDto.PreMade, Is.EqualTo(expectedBool));
    }

    [TestCase(false, 0)]
    [TestCase(true, 1)]
    public void MapToDto_MapsCorrectBool(bool premade, int expectedBoolInteger)
    {
        WorkoutDto workoutDto  = new() { Id = 1, Description = "Description", Name = "Name", PreMade = premade };

        //Act
        var returnWorkoutModel = target.MapToDatabase(workoutDto);

        //Assert
        Assert.That(returnWorkoutModel.Id, Is.EqualTo(1));
        Assert.That(returnWorkoutModel.Description, Is.EqualTo("Description"));
        Assert.That(returnWorkoutModel.Name, Is.EqualTo("Name"));
        Assert.That(returnWorkoutModel.PreMade, Is.EqualTo(expectedBoolInteger));
    }

}