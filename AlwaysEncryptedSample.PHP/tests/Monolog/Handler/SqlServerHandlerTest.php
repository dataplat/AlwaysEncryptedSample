<?php
declare(strict_types = 1);

namespace SqlCollaborative\AlwaysEncryptedSample\Monolog\Handler;

use Monolog\Logger;
use PDO;
use PDOException;
use PHPUnit\Framework\TestCase;

class SqlServerHandlerTest extends TestCase
{
    private $dsn =
        'odbc:Driver={ODBC Driver 13 for SQL Server};Server=localhost,1433;Database=AlwaysEncryptedSample;' .
        'UID=sa;PWD=alwaysB3Encrypt1ng;ColumnEncryption=Enabled;APP=PHP Unit -- ALwaysEncrypted Sample;';
    /**
     * @var PDO
     */
    private $connection;
    /**
     * @var SqlServerHandler $handler
     */
    private $handler;

    public function setup()
    {
        $this->handler = new SqlServerHandler($this->dsn);
        $this->connection = new PDO($this->dsn);
        $this->connection->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    }

    /**
     * @param int $level
     * @param string $message
     * @param array $context
     * @return array Record
     */
    protected function getRecord($level = Logger::WARNING, $message = 'test', $context = array())
    {
        return array(
            'message' => $message,
            'context' => $context,
            'level' => $level,
            'level_name' => Logger::getLevelName($level),
            'channel' => 'test',
            'datetime' => new \DateTime(),
            'extra' => array(),
        );
    }

    /**
     * @expectedException PDOException
     * @expectedExceptionMessage could not find driver
     */
    public function testBogusDsn()
    {
        $badDsn = 'zippydb:name=Not The Greatest Db;Other=Just a Tribute;';
        new SqlServerHandler($badDsn);
    }

    /**
     * @expectedException PDOException
     * @expectedExceptionMessage No connection could be made because the target machine actively refused it.
     */
    public function testBadSqlServerPort()
    {
        $badDsn = 'odbc:Driver={ODBC Driver 13 for SQL Server};Server=localhost,9999;Database=AlwaysEncryptedSample;' .
            'UID=sa;PWD=alwaysB3Encrypt1ng;ColumnEncryption=Enabled;APP=PHP Unit -- ALwaysEncrypted Sample';
        new SqlServerHandler($badDsn);
    }

    /**
     * @expectedException PDOException
     * @expectedExceptionMessage SQLSTATE[28000] SQLDriverConnect: 18456 [Microsoft][ODBC Driver 13 for SQL Server][SQL Server]Login failed for user 'sa'
     */
    public function testBadDb()
    {
        $badDsn = 'odbc:Driver={ODBC Driver 13 for SQL Server};Server=localhost,1433;Database=Not A Real Db;' .
            'UID=sa;PWD=alwaysB3Encrypt1ng;ColumnEncryption=Enabled;APP=PHP Unit -- ALwaysEncrypted Sample';
        new SqlServerHandler($badDsn);
    }

    public function testWrite()
    {
        $record = $this->getRecord();
        $this->handler->handle($record);
        $sql = <<<EOSQL
DECLARE @timestamp VARCHAR(35) = :datetime; 
SELECT COUNT(*) FROM Logging.Log WHERE [Date] = @timestamp AND message = :message;
EOSQL;

        $this->connection->beginTransaction();
        $stmt = $this->connection->prepare($sql);
        $stmt->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        $stmt->bindValue(':datetime', $record['datetime']->format(DATE_ATOM));
        $stmt->bindValue(':message', $record['message']);

        $stmt->execute();
        $this->assertEquals(1, $stmt->fetchColumn());
        $this->connection->rollBack();

        $stmt = $this->connection->prepare($sql);
        $stmt->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        $stmt->bindValue(':datetime', $record['datetime']->format(DATE_ATOM));
        $stmt->bindValue(':message', $record['message']);
        $this->assertEquals(false, $stmt->fetchColumn(), 'Rollback didn\'t work.');
    }
}