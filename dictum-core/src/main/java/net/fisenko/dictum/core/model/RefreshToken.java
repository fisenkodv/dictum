package net.fisenko.dictum.core.model;

import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

@Getter
@Setter
@ToString
public class RefreshToken {

    private String id;
    private String userName;
    private String refreshToken;
    private boolean revoked;
}
