package net.fisenko.dictum.micronaut.dto.quote;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import javax.validation.constraints.NotBlank;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
public final class CreateQuote {

    @NotBlank
    private String authorId;
    @NotBlank
    private String text;
}
