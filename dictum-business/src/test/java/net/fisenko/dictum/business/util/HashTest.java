package net.fisenko.dictum.business.util;

import org.junit.jupiter.api.Test;

import static org.assertj.core.api.AssertionsForClassTypes.assertThat;

public class HashTest {

    @Test
    public void shouldCreateExpectedSHA256() {
        final String hash = Hash.sha256("Stay hungry, stay foolish.");

        assertThat(hash).isEqualTo("1406867876646022f065d8f0b34179dd1df113e5db866c8920987f320f9bad6f");
    }
}
