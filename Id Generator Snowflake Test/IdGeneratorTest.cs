using Id_Generator_Snowflake;

namespace Id_Generator_Snowflake_Test;

public class IdGeneratorTest
{
    [Fact]
    public void IdGenerator_Constructor_ValidParameters_ShouldInitializeCorrectly()
    {
        // Arrange
        var wkrId = 1;
        var dcId = 1;
        var seq = 1;

        // Act
        var idGen = new IdGenerator(wkrId, dcId, seq);

        // Assert
        Assert.Equal(wkrId, idGen.WorkerId);
        Assert.Equal(dcId, idGen.DatacenterId);
        Assert.Equal(seq, idGen.Sequence);
    }

    [Fact]
    public void IdGenerator_Constructor_InvalidWorkerId_ShouldThrowArgumentException()
    {
        // Arrange
        var invWkrId = -1;
        var dcId = 1;
        var seq = 1;

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new IdGenerator(invWkrId, dcId, seq));
        Assert.Contains("Worker Id", ex.Message);
    }

    [Fact]
    public void IdGenerator_Constructor_InvalidDatacenterId_ShouldThrowArgumentException()
    {
        // Arrange
        var wkrId = 1;
        var invDcId = -1;
        var seq = 1;

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new IdGenerator(wkrId, invDcId, seq));
        Assert.Contains("Datacenter Id", ex.Message);
    }

    [Fact]
    public void NextId_GenerateUniqueId_ShouldReturnUniqueIds()
    {
        // Arrange
        var wkrId = 1;
        var dcId = 1;
        var idGen = new IdGenerator(wkrId, dcId);

        // Act
        var id1 = idGen.NextId();
        var id2 = idGen.NextId();

        // Assert
        Assert.NotEqual(id1, id2);
    }

    [Fact]
    public void NextIdAlphabetic_GenerateUniqueId_ShouldReturnUniqueAlphabeticIds()
    {
        // Arrange
        var wkrId = 1;
        var dcId = 1;
        var idGen = new IdGenerator(wkrId, dcId);

        // Act
        var id1 = idGen.NextIdAlphabetic();
        var id2 = idGen.NextIdAlphabetic();

        // Assert
        Assert.NotEqual(id1, id2);
    }

    [Fact]
    public void ExtractIdComponents_ValidId_ShouldReturnCorrectComponents()
    {
        // Arrange
        var ts = DateTime.UtcNow;
        var wkrId = 0;
        var dcId = 1;
        var seq = 2;
        var tsEpoch = 1_720_569_600_000;
        var idGen = new IdGenerator(wkrId, dcId, seq);
        var id = idGen.NextId(tsEpoch);

        // Act
        var (Timestamp, WorkerId, DatacenterId) = IdGenerator.ExtractIdComponents(id, tsEpoch, seq);

        // Assert
        Assert.True(Math.Abs((Timestamp - ts).TotalMilliseconds) < TimeSpan.FromMilliseconds(1_000 / seq).TotalMilliseconds, "Timestamp does not match.");
        Assert.Equal(wkrId, WorkerId);
        Assert.Equal(dcId, DatacenterId);
    }

    [Fact]
    public void ExtractIdAlphabeticComponents_ValidAlphabeticId_ShouldReturnCorrectComponents()
    {
        // Arrange
        var ts = DateTime.UtcNow;
        var wkrId = 0;
        var dcId = 1;
        var seq = 2;
        var tsEpoch = 1_720_569_600_000;
        var idGen = new IdGenerator(wkrId, dcId, seq);
        var id = idGen.NextIdAlphabetic(tsEpoch);

        // Act
        var (Timestamp, WorkerId, DatacenterId) = IdGenerator.ExtractIdAlphabeticComponents(id, tsEpoch, seq);

        // Assert
        Assert.True(Math.Abs((Timestamp - ts).TotalMilliseconds) < TimeSpan.FromMilliseconds(1_000 / seq).TotalMilliseconds, "Timestamp does not match.");
        Assert.Equal(wkrId, WorkerId);
        Assert.Equal(dcId, DatacenterId);
    }
}
