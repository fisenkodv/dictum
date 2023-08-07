package net.fisenko.dictum.micronaut.dto.like;

import lombok.*;
import net.fisenko.dictum.business.annotation.Id;

import jakarta.validation.constraints.NotBlank;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
public final class LikeQuote {

    @Id
    @NotBlank
    private String quoteId;
}

