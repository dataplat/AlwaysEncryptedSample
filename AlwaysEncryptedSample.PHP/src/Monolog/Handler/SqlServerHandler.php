<?php
declare(strict_types = 1);

namespace SqlCollaborative\AlwaysEncryptedSample\Monolog\Handler;

use PDOException;
use Monolog\Handler\AbstractProcessingHandler;
use Monolog\Logger;
use PDO;
use PDOStatement;

class SqlServerHandler extends AbstractProcessingHandler
{

    /**
     * @var PDO $connection
     */
    private $connection;
    /**
     * @var string $logger Name of the Logger to record in the Logger column.
     */
    protected $logger;
    /**
     * @var int $pdoErrorMode the error mode of the connection and statement objects.
     */
    private $pdoErrorMode;

    private $sqlInsert;
    /**
     * @var PDOStatement $statement The prepared PDO insert statement.
     */
    private $statement;


    /**
     * SqlServerHandler constructor.
     * @param string $dsn The DSN to connect to.
     * @param bool|int $level
     * @param bool $bubble
     * @param null $loggerName name of the logger
     */
    public function __construct(
        $dsn,
        $level = Logger::DEBUG,
        $bubble = true,
        $loggerName = null
    ) {
        try {
            $this->connection = new PDO($dsn);
        }
        catch (PDOException $ex){
            throw new \RuntimeException("Error connecting to DSN $dsn", 0, $ex);
        }

        $this
            ->setLoggerName($loggerName ?? 'Monolog')
            ->setPdoErrorMode(PDO::ERRMODE_EXCEPTION)
            ->setSqlInsert(<<< EOSQL
DECLARE @timestamp VARCHAR(35) = :datetime;
INSERT INTO Logging.Log
  ([Date],[Level],[Message], [Logger] /*, [User], [ClientIP]*/)
  VALUES (@timestamp, :level_name, :message, :logger /*, @user, @client_ip */);
EOSQL
            );
        parent::__construct($level, $bubble);
    }

    public function setLoggerName(string $logger) : self
    {
        $this->logger = $logger;
        return $this;
    }

    public function setPdoAttribute(int $attribute, $value) : self
    {
        $this->connection->setAttribute($attribute, $value);
        return $this;
    }

    /**
     * Sets the attribute error mode
     * @param int $errorMode
     * @return SqlServerHandler
     */
    public function setPdoErrorMode(int $errorMode = PDO::ERRMODE_EXCEPTION) : self
    {
        $this->pdoErrorMode = $errorMode;
        return $this->setPdoAttribute(PDO::ATTR_ERRMODE, $errorMode);
    }

    /**
     * @param string $sqlInsert
     * @return SqlServerHandler
     */
    public function setSqlInsert($sqlInsert) : self
    {
        $this->sqlInsert = $sqlInsert;
        $this->statement = $this->connection->prepare($this->sqlInsert);
        //TODO: this isn't working ask on Stackoverflow or read the source to figure out why.
        $this->statement->setAttribute(PDO::ATTR_ERRMODE, $this->pdoErrorMode);

        return $this;
    }

    /**
     * Writes the record down to the log of the implementing handler
     *
     * @param  array $record
     * @return void
     */
    protected function write(array $record)
    {
        //TODO: SQL server chokes on DATE_ATOM because of the timezone offset
        //TODO: I need to make thread not null.
        try {
            $this->statement->execute([
                ':datetime' => $record['datetime']->format(DATE_ATOM),
                ':level_name' => $record['level_name'],
                ':message' => $record['message'],
                ':logger' => $this->logger,
            ]);
        } catch (PDOException $e) {
            echo sprintf("Date: \"%s\"", $record['datetime']->format(DATE_ATOM));
            //$this->statement->debugDumpParams();
            throw $e;
        }
    }
}