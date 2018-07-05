namespace Flights.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommercialFlights",
                c => new
                    {
                        IdFlight = c.String(nullable: false, maxLength: 128),
                        Distance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deport = c.DateTime(nullable: false),
                        Arrive = c.DateTime(nullable: false),
                        Price = c.Single(nullable: false),
                        Disponible_Places = c.Int(nullable: false),
                        FlightTo = c.String(maxLength: 128),
                        FlightFrom = c.String(maxLength: 128),
                        Flight_Plane = c.String(maxLength: 128),
                        Destination_ZipCode = c.String(maxLength: 128),
                        Destination_ZipCode1 = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IdFlight)
                .ForeignKey("dbo.Destinations", t => t.Destination_ZipCode)
                .ForeignKey("dbo.Destinations", t => t.Destination_ZipCode1)
                .ForeignKey("dbo.Destinations", t => t.FlightFrom)
                .ForeignKey("dbo.Planes", t => t.Flight_Plane)
                .ForeignKey("dbo.Destinations", t => t.FlightTo)
                .Index(t => t.FlightTo)
                .Index(t => t.FlightFrom)
                .Index(t => t.Flight_Plane)
                .Index(t => t.Destination_ZipCode)
                .Index(t => t.Destination_ZipCode1);
            
            CreateTable(
                "dbo.FlightPlaces",
                c => new
                    {
                        numPlace = c.Int(nullable: false),
                        idFlight = c.String(nullable: false, maxLength: 128),
                        Place_Owner_Name = c.String(),
                        Place_Owner_DNI = c.Int(nullable: false),
                        FP_Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.numPlace, t.idFlight })
                .ForeignKey("dbo.CommercialFlights", t => t.idFlight, cascadeDelete: true)
                .Index(t => t.idFlight);
            
            CreateTable(
                "dbo.Destinations",
                c => new
                    {
                        ZipCode = c.String(nullable: false, maxLength: 128),
                        idAirport = c.String(),
                        NameAirport = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                    })
                .PrimaryKey(t => t.ZipCode);
            
            CreateTable(
                "dbo.Planes",
                c => new
                    {
                        idPlane = c.String(nullable: false, maxLength: 128),
                        namePlane = c.String(),
                        CrossingRange = c.Single(nullable: false),
                        PlanePlaces = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.idPlane);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommercialFlights", "FlightTo", "dbo.Destinations");
            DropForeignKey("dbo.CommercialFlights", "Flight_Plane", "dbo.Planes");
            DropForeignKey("dbo.CommercialFlights", "FlightFrom", "dbo.Destinations");
            DropForeignKey("dbo.CommercialFlights", "Destination_ZipCode1", "dbo.Destinations");
            DropForeignKey("dbo.CommercialFlights", "Destination_ZipCode", "dbo.Destinations");
            DropForeignKey("dbo.FlightPlaces", "idFlight", "dbo.CommercialFlights");
            DropIndex("dbo.FlightPlaces", new[] { "idFlight" });
            DropIndex("dbo.CommercialFlights", new[] { "Destination_ZipCode1" });
            DropIndex("dbo.CommercialFlights", new[] { "Destination_ZipCode" });
            DropIndex("dbo.CommercialFlights", new[] { "Flight_Plane" });
            DropIndex("dbo.CommercialFlights", new[] { "FlightFrom" });
            DropIndex("dbo.CommercialFlights", new[] { "FlightTo" });
            DropTable("dbo.Planes");
            DropTable("dbo.Destinations");
            DropTable("dbo.FlightPlaces");
            DropTable("dbo.CommercialFlights");
        }
    }
}
