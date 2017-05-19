<?php
declare(strict_types = 1);

namespace SqlCollaborative\AlwaysEncryptedSample\Security;

use RuntimeException;

/**
 * Class AspNetIdentity
 * @package SqlCollaborative\AlwaysEncryptedSample\Security
 * A direct port of the algorithims to generate and verify ASP.NET password hashes found here:
 * @link https://aspnetidentity.codeplex.com/SourceControl/latest#src/Microsoft.AspNet.Identity.Core/Crypto.cs
 * For mor information:
 * @link http://stackoverflow.com/questions/20621950/asp-net-identity-default-password-hasher-how-does-it-work-and-is-it-secure
 */
class AspNetIdentity
{
    const PBKDF2_ITER_COUNT = 1000; // default for Rfc2898DeriveBytes
    const PBKDF2_SUBKEY_LENGTH = 256/8; // 256 bits
    const SALT_SIZE = 128/8; // 128 bits

    public function HashPassword(string $password) : string
    {
        // Produce a version 0 (see comment above) text hash.
        $salt = [];
        $subkey = [];
        $hash = hash_hmac(
            'sha1',  //TODO: Is SHA1 Broken? Can I make ASP.NET use another algo?
            $password,
            random_bytes(self::SALT_SIZE)
        );

        return base64_encode($hash);
    }

        // hashedPassword must be of the format of HashWithPassword (salt + Hash(salt+input)
        public function VerifyHashedPassword(string $hashedPassword, string $password) : bool
        {
            throw new RuntimeException('Unimplemented');
            $hashedPasswordByteString = base64_decode($hashedPassword);
            $expectedHashLength = 1 + self::SALT_SIZE + self::PBKDF2_SUBKEY_LENGTH;
            $actualHashLength = strlen($hashedPasswordByteString);
            if ($actualHashLength != $expectedHashLength)
            {
                trigger_error(
                    "Salt is wrong length [ expected = {$expectedHashLength}, actual = {$actualHashLength} ]"
                );
                return false;
            }
            $header = substr($hashedPasswordByteString, 0,2);

/*
            if ($header != '00'){
                trigger_error("Incorrect header [ $header ]");
            }

            $salt = substr($hashedPasswordByteString, 1, self::SALT_SIZE);
            Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, SaltSize);
            var storedSubkey = new byte[PBKDF2SubkeyLength];
            Buffer.BlockCopy(hashedPasswordBytes, 1 + SaltSize, storedSubkey, 0, PBKDF2SubkeyLength);

            byte[] generatedSubkey;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, PBKDF2IterCount))
            {
                generatedSubkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
            }
            return ByteArraysEqual(storedSubkey, generatedSubkey);
 */
        }

}