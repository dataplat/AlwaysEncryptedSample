<?php
declare(strict_types = 1);

namespace SqlCollaborative\AlwaysEncryptedSample\Security;

use RuntimeException;

/**
 * Class AspNetIdentity
 * @package SqlCollaborative\AlwaysEncryptedSample\Security
 * A direct port of the algorithm to generate and verify ASP.NET password hashes found here:
 * @link https://aspnetidentity.codeplex.com/SourceControl/latest#src/Microsoft.AspNet.Identity.Core/Crypto.cs
 * For mor information:
 * @link http://stackoverflow.com/questions/20621950/asp-net-identity-default-password-hasher-how-does-it-work-and-is-it-secure
 */
class AspNetIdentity
{
    const PBKDF2_ITER_COUNT = 1000; // default for Rfc2898DeriveBytes
    const PBKDF2_SUBKEY_LENGTH = 256/8; // 256 bits
    const SALT_SIZE = 128/8; // 128 bits

    private function generateRandomSalt() : string
    {
        return random_bytes(self::SALT_SIZE);
    }

    public function getPasswordSalt(string $hashedPassword) : string
    {
        $hashedPasswordByteString = base64_decode($hashedPassword);
        $expectedHashLength = 1 + self::SALT_SIZE + self::PBKDF2_SUBKEY_LENGTH;
        $actualHashLength = strlen($hashedPasswordByteString);

        $header = substr($hashedPasswordByteString, 0, 1);
        if ($header != "\0"){
            throw new RuntimeException("Incorrect header [ $header ]");
        }

        if ($actualHashLength != $expectedHashLength)
        {
            throw new RuntimeException (
                "Salted hash is wrong length [ expected = {$expectedHashLength}, actual = {$actualHashLength} ]"
            );
        }

        return substr($hashedPasswordByteString, 1, self::SALT_SIZE);
    }

    /**
     * Creates a PBKDF2 (AKA Rfc2898) hash from a plaintext password.
     * @param string $password A plaintext password
     * @param string|null $salt Optionally specify the hash. This should only be used to verify existin passwords.
     * @return string The base64 encoded password hash
     */
    public function hashPassword(string $password, string $salt = null) : string
    {
        $salt = $salt ?? $this->generateRandomSalt();
        $subkey = hash_pbkdf2(
            'sha1',  // The SHA1 exploit google discovered is irrevelevant to HMAC_SHA1
            $password,
            $salt,
            self::PBKDF2_ITER_COUNT,
            self::PBKDF2_SUBKEY_LENGTH,
            true
        );

        return base64_encode("\0" . $salt . $subkey);
    }

    /**
     * verifies a plaintext password matches its hash.
     * @param string $hashedPassword must be of the format of HashWithPassword (salt + Hash(salt+input)
     * @param string $password the plain text version of the password
     * @return bool true if the password matches false if it does not.
     */
    public function verifyHashedPassword(string $hashedPassword, string $password) : bool
    {
        try {
            $salt = $this->getPasswordSalt($hashedPassword);
        }
        catch (RuntimeException $ex) {
            trigger_error($ex->getMessage());
            return false;
        }
        $actualHashedPassword = $this->hashPassword($password, $salt);
        return ($actualHashedPassword === $hashedPassword);
    }
}