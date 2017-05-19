<?php
declare(strict_types = 1);

namespace SqlCollaborative\AlwaysEncryptedSample\Util;

/**
 * Generates a fake social security number not in the range of social security numbers actually generated.
 * @link  http://stackoverflow.com/questions/2313704/is-there-a-social-security-number-reserved-for-testing-examples
 */
class SSNGenerator
{
    private $testAreaGroups = null;

    public function __construct()
    {
        $this->initValidAreaGroups();
    }

    private function initValidAreaGroups()
    {
        $this->testAreaGroups = [
            '000',
            '666',
        ];
        for($i = 900; $i <= 999; $i++) {
            $this->testAreaGroups[] = (string)$i;
        }

    }

    private function generateAreaGroup() : string
    {
        return $this->testAreaGroups[random_int(0, count($this->testAreaGroups))];
    }

    private function generateGroupNumber() : string
    {
        return str_pad((string) random_int(1, 99), 2, '0');
    }

    private function generateSerialNumber() : string
    {
        return str_pad((string) random_int(1, 9999), 4, '0');
    }

    public function generateSSN() : string
    {
        return sprintf(
            "%s-%s-%s",
            $this->generateAreaGroup(),
            $this->generateGroupNumber(),
            $this->generateSerialNumber()
        );
    }
}