namespace ApiContracts;

public record UserCreateDto(string UserName);
public record UserUpdateDto(int Id, string UserName);
public record UserDto(int Id, string UserName);