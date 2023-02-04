### Architecture Decisions
---
#### Folder Structure
    📦 src
    ┣ 📁 Project
    ┃ ┗ 📁 ProjectSolution
    ┃ ┗ 📄 Project.sln
    ┣ 📁 Test_Project
    ┃ ┗ 📄 Test_Example.cs
    ┣ 📁 BDD
    ┃ ┗ 📄 gherkin.md
    ┃ ┗ 📄 README.md
    ┗ 📁 Jest
     ​​​​ ​​┗ 📄 example.test.js
      ┗ 📄 README.md

#### .Net Version
    .NET 7.0.100

#### Front-end Libraries
    Bootstrap v5.3

#### Javascript
    Vanilla JS 

#### Git branch naming
    'Story#-FeatureName-Author'

#### DB Convention 
    Given Example

    # make table
    CREATE TABLE [Character] (
        [ID] int Primary Key Identity(1, 1),
        [Name] nvarchar(30) NOT NULL,
        [Level] int NOT NULL,
        [CharacterClassID] int NOT NULL, 
    );
    
    # declare foreign key
    ALTER TABLE [Character] ADO FOREIGN KEY ([CharacterClassID]) REFERENCES [Characterclass] ([101]) ;

    # create constraint
    ALTER TABLE [Character] ADD CONSTRAINT [Fk Character Class ID]
        FOREIGN KEY ([CharacterClassID]) REFERENCES [CharacterClass] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

    # seed table
    INSERT INTO [Character] (Created, Level, Health, CharacterClassID, Name)
        VALUES
        ('2021-12-04 12:15:44',3 ,40 ,1, 'Millia Rage'),
        (2021-12-03 15:00:00 ,9 23 ,3,'Haruka Sawamura") 2021-12-02 03:56:11 15,75 5, 'Athena Asamiya"),
        ('2021-11-25 07:23:08,11,52 ,5, 'Jak'),
        ('2021-11-25 20:10:231,22,300,6, 'Kanji Tatsumi");


#### Entity Loading Type
    Lazy loading