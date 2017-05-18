<?php
/**
 * Created by PhpStorm.
 * User: zippy
 * Date: 2017-05-17
 * Time: 23:44
 */

namespace SqlCollaborative\AlwaysEncryptedSample\Command;


use SqlCollaborative\AlwaysEncryptedSample\Util\SSNGenerator;
use Symfony\Component\Console\Command\Command;
use Symfony\Component\Console\Input\InputInterface;
use Symfony\Component\Console\Output\OutputInterface;

class GenerateSSN extends Command
{
    protected function configure()
    {
        $this
            ->setName("generate:SSN")
            ->setDescription("Generate a fake social security number guaranteed not to overlap with real ones.");
    }

    protected function execute(InputInterface $input, OutputInterface $output)
    {
        $ssnGenerator = new SSNGenerator();
        $newline = ! $input->getOption('supress-newline');
        $output->write($ssnGenerator->generateSSN());
    }
    }