<?php
declare(strict_types = 1);

namespace SqlCollaborative\AlwaysEncryptedSample\Security;

use PHPUnit\Framework\TestCase;
use RuntimeException;

final class AspNetIdentityTest extends TestCase
{
    public function testPassword() {
        $aspNetIdentity = new AspNetIdentity();
        $password = random_bytes(256);
        $hash = $aspNetIdentity->hashPassword($password);
        $this->assertTrue($aspNetIdentity->verifyHashedPassword($hash, $password));
    }

    public function testVerifyBadHashPassword() {
        $aspNetIdentity = new AspNetIdentity();
        $badHash = base64_encode('XXXXXX');
        @$this->assertFalse($aspNetIdentity->verifyHashedPassword('password', $badHash));
    }

    /**
     * @expectedException RuntimeException
     * @expectedExceptionMessage Salted hash is wrong length [ expected = 49, actual =
     */
    public function testBadSalt() {
        $aspNetIdentity = new AspNetIdentity();
        $salt = base64_encode("\0" . random_bytes(256));
        $aspNetIdentity->getPasswordSalt($salt);
    }

    /**
     * @expectedException RuntimeException
     * @expectedExceptionMessage Incorrect header [ X ]
     */
    public function testBadHeader() {
        $aspNetIdentity = new AspNetIdentity();
        $salt = base64_encode('XXXXXX');
        $aspNetIdentity->getPasswordSalt($salt);
    }
}