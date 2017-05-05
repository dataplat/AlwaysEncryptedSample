<?php
declare(strict_types = 1);

namespace SqlCollaborative\AlwaysEncryptedSample\Monolog\Handler;

use Monolog\TestCase;
use PDO;

class SqlServerHandlerTest extends \PHPUnit_Framework_TestCase
{
    private $dsn =
        'odbc:Driver={ODBC Driver 13 for SQL Server};Server=.;' .
        'Trusted_Connection=yes;ColumnEncryption=Enabled;APP=AlwaysEncrytedSample PHP';
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
        $sql = "SELECT COUNT(*) FROM Logging.Log WHERE [Date] = @date AND message = @message";
        $stmt = $this->connection->prepare($sql);
        $stmt->bindParam('@date', $record['datetime']);
        $stmt->bindParam('@message', $record['message']);

        $stmt->execute();
        $this->assertEquals('1', $stmt->fetchColumn());
    }
}