CREATE TABLE Sales (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    SalesAmount DECIMAL(18, 2),
    CategoryId INT,
    LocationId INT,
    SalesDate DATE,
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    FOREIGN KEY (LocationId) REFERENCES Locations(Id)
);

CREATE TABLE CategoryRelationships (
    AncestorId INT,
    DescendantId INT,
    Depth INT, -- Depth between ancestor and descendant
    FOREIGN KEY (AncestorId) REFERENCES Categories(Id),
    FOREIGN KEY (DescendantId) REFERENCES Categories(Id),
    PRIMARY KEY (AncestorId, DescendantId)
);

CREATE TABLE LocationRelationships (
    AncestorId INT,
    DescendantId INT,
    Depth INT, -- Depth between ancestor and descendant
    FOREIGN KEY (AncestorId) REFERENCES Locations(Id),
    FOREIGN KEY (DescendantId) REFERENCES Locations(Id),
    PRIMARY KEY (AncestorId, DescendantId)
);

CREATE TABLE Categories (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(100),
    Level INT
);

CREATE TABLE Locations (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(100),
    Level INT -- 0 for country, 1 for region, 2 for city, etc.
);