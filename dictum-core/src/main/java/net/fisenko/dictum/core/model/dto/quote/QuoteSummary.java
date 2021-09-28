package net.fisenko.dictum.core.model.dto.quote;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import net.fisenko.dictum.core.model.dto.author.AuthorSummary;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
public final class QuoteSummary {

    private String id;
    private String text;
    private AuthorSummary author;
}