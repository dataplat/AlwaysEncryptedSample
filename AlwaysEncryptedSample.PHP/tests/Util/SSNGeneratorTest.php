<?php
declare(strict_types = 1);

namespace SqlCollaborative\AlwaysEncryptedSample\Util;

use PHPUnit\Framework\TestCase;

class SSNGeneratorTest extends TestCase
{

    /**
     * @var string $validSsnRegex
     */
    private $validSsnRegex = '/^(?!(000|666|9))\d{3}-(?!00)\d{2}-(?!0000)\d{4}$/';
    private $niaveSsnRegex = '/^\d{3}-\d{2}-\d{4}$/';
    /**
     * @var SSNGenerator $ssnGenerator
     */
    private $ssnGenerator;

    public function setup()
    {
        $this->ssnGenerator = new SSNGenerator();
    }

    public function testSsn()
    {
        $ssn = $this->ssnGenerator->generateSSN();
        $this->assertEquals(11, strlen($ssn));
        $this->assertRegExp($this->niaveSsnRegex, $ssn, 'Generated SSN not in the correct format: ' . $ssn);
        $this->assertNotRegExp($this->validSsnRegex, $ssn, 'Generated SSN is in the valid range: ' . $ssn);
    }

}