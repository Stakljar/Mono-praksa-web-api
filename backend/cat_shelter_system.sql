CREATE TABLE "CatShelter" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "Name" VARCHAR(300) NOT NULL,
    "Location" VARCHAR(400) NOT NULL,
    "EstablishedAt" DATE NOT NULL
);

CREATE TABLE "Cat" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "Name" VARCHAR(50) NOT NULL,
    "Age" INT NOT NULL,
    "Color" VARCHAR(50) NOT NULL,
    "ArrivalDate" DATE,
	"CatShelterId" UUID,
    CONSTRAINT "FK_Cat_CatShelter_CatShelterId" FOREIGN KEY ("CatShelterId") REFERENCES "CatShelter"("Id")
		ON UPDATE CASCADE ON DELETE SET NULL
);
