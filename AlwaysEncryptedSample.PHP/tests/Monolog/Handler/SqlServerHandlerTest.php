<?php
declare(strict_types = 1);

namespace SqlCollaborative\AlwaysEncryptedSample\Monolog\Handler;

use Monolog\Logger;
use PDO;
use PHPUnit_Framework_TestCase;

class SqlServerHandlerTest extends PHPUnit_Framework_TestCase
{
    private $dsn =
        'odbc:Driver={ODBC Driver 13 for SQL Server};Server=localhost,1433;Database=AlwaysEncryptedSample;' .
        'UID=sa;PWD=alwaysB3Encrypt1ng;ColumnEncryption=Enabled;APP=PHP Unit -- ALwaysEncrypted Sample';
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
            'datetime' => \DateTime::createFromFormat('U.u', sprintf('%.6F', microtime(true))),
            'extra' => array(),
        );
    }

    public function testWrite()
    {
        $record = $this->getRecord();
        $this->handler->handle($record);
        $sql = <<<EOSQL
DECLARE @timestamp VARCHAR(35) = :datetime; 
SELECT COUNT(*) FROM Logging.Log WHERE [Date] = @timestamp AND message = :message;
EOSQL;

        $stmt = $this->connection->prepare($sql);
        $stmt->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        $stmt->bindValue(':datetime', $record['datetime']->format(DATE_ATOM));
        $stmt->bindValue(':message', $record['message']);

        $stmt->execute();
        $this->assertEquals('1', $stmt->fetchColumn());
    }
}