package net.fisenko.dictum.micronaut.dto.author;

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
public final class AuthorDetail {

    private String id;
    private String name;
    private String bio;
}
