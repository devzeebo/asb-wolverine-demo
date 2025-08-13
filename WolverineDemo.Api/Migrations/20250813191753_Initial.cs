using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WolverineDemo.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema("Wolverine");
            migrationBuilder.Sql(@"
CREATE TABLE Wolverine.wolverine_outgoing_envelopes (
    id              uniqueidentifier    NOT NULL,
    owner_id        int                 NOT NULL,
    destination     varchar(250)        NOT NULL,
    deliver_by      datetimeoffset      NULL,
    body            varbinary(max)      NOT NULL,
    attempts        int                 NULL DEFAULT 0,
    message_type    varchar(250)        NOT NULL,
CONSTRAINT pkey_wolverine_outgoing_envelopes_id PRIMARY KEY (id)
);

CREATE TABLE Wolverine.wolverine_incoming_envelopes (
    id                uniqueidentifier    NOT NULL,
    status            varchar(25)         NOT NULL,
    owner_id          int                 NOT NULL,
    execution_time    datetimeoffset      NULL DEFAULT NULL,
    attempts          int                 NULL DEFAULT 0,
    body              varbinary(max)      NOT NULL,
    message_type      varchar(250)        NOT NULL,
    received_at       varchar(250)        NOT NULL,
    keep_until        datetimeoffset      NULL,
CONSTRAINT pkey_wolverine_incoming_envelopes_id PRIMARY KEY (id)
);

CREATE TABLE Wolverine.wolverine_dead_letters (
    id                   uniqueidentifier    NOT NULL,
    execution_time       datetimeoffset      NULL DEFAULT NULL,
    body                 varbinary(max)      NOT NULL,
    message_type         varchar(250)        NOT NULL,
    received_at          varchar(250)        NOT NULL,
    source               varchar(250)        NULL,
    exception_type       varchar(max)        NULL,
    exception_message    varchar(max)        NULL,
    sent_at              datetimeoffset      NULL,
    replayable           bit                 NULL,
CONSTRAINT pkey_wolverine_dead_letters_id PRIMARY KEY (id)
);

CREATE TABLE Wolverine.wolverine_nodes (
    id              uniqueidentifier    NOT NULL,
    node_number     int                 NOT NULL IDENTITY,
    description     varchar(max)        NOT NULL,
    uri             varchar(500)        NOT NULL,
    started         datetimeoffset      NOT NULL DEFAULT GETUTCDATE(),
    health_check    datetimeoffset      NOT NULL DEFAULT GETUTCDATE(),
    version         varchar(100)        NULL,
    capabilities    nvarchar(max)       NULL,
CONSTRAINT pkey_wolverine_nodes_id PRIMARY KEY (id)
);

CREATE TABLE Wolverine.wolverine_node_assignments (
    id         varchar(500)        NOT NULL,
    node_id    uniqueidentifier    NULL,
    started    datetimeoffset      NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT pkey_wolverine_node_assignments_id PRIMARY KEY (id),
    CONSTRAINT fkey_wolverine_node_assignments_node_id FOREIGN KEY(node_id)
        REFERENCES Wolverine.wolverine_nodes(id) ON DELETE CASCADE
);

CREATE TABLE Wolverine.wolverine_control_queue (
    id              uniqueidentifier    NOT NULL,
    message_type    varchar(100)        NOT NULL,
    node_id         uniqueidentifier    NOT NULL,
    body            varbinary(max)      NOT NULL,
    posted          datetimeoffset      NOT NULL DEFAULT GETUTCDATE(),
    expires         datetimeoffset      NULL,
CONSTRAINT pkey_wolverine_control_queue_id PRIMARY KEY (id)
);

CREATE TABLE Wolverine.wolverine_node_records (
    id             int               NOT NULL IDENTITY,
    node_number    int               NOT NULL,
    event_name     varchar(500)      NOT NULL,
    timestamp      datetimeoffset    NOT NULL DEFAULT GETUTCDATE(),
    description    varchar(500)      NULL,
CONSTRAINT pkey_wolverine_node_records_id PRIMARY KEY (id)
);

CREATE TYPE Wolverine.EnvelopeIdList AS TABLE (id UNIQUEIDENTIFIER NOT NULL);

go

CREATE PROCEDURE Wolverine.uspDeleteIncomingEnvelopes
    @IDLIST Wolverine.EnvelopeIdList READONLY
AS

DELETE
FROM Wolverine.wolverine_incoming_envelopes
WHERE id IN (SELECT ID FROM @IDLIST);

GO

CREATE PROCEDURE Wolverine.uspDeleteOutgoingEnvelopes
    @IDLIST Wolverine.EnvelopeIdList READONLY
AS

DELETE
FROM Wolverine.wolverine_outgoing_envelopes
WHERE id IN (SELECT ID FROM @IDLIST);

GO

CREATE PROCEDURE Wolverine.uspDiscardAndReassignOutgoing
    @DISCARDS Wolverine.EnvelopeIdList READONLY,
    @REASSIGNED Wolverine.EnvelopeIdList READONLY,
    @OWNERID INT

AS
DELETE
FROM Wolverine.wolverine_outgoing_envelopes
WHERE id IN (SELECT ID FROM @DISCARDS);

UPDATE Wolverine.wolverine_outgoing_envelopes
SET owner_id = @OWNERID
WHERE ID IN (SELECT ID FROM @REASSIGNED);

GO

CREATE PROCEDURE Wolverine.uspMarkIncomingOwnership
    @IDLIST Wolverine.EnvelopeIdList READONLY,
    @owner INT
AS

UPDATE Wolverine.wolverine_incoming_envelopes
SET owner_id = @owner, status = 'Incoming'
WHERE id IN (SELECT ID FROM @IDLIST);

GO

CREATE PROCEDURE Wolverine.uspMarkOutgoingOwnership
    @IDLIST Wolverine.EnvelopeIdList READONLY,
    @owner INT
AS

UPDATE Wolverine.wolverine_outgoing_envelopes
SET owner_id = @owner
WHERE id IN (SELECT ID FROM @IDLIST);

GO
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
