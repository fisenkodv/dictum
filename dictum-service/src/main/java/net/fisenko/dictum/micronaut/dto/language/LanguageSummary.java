package net.fisenko.dictum.micronaut.dto.language;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
public final class LanguageSummary {

    private String code;
    private String language;
}
