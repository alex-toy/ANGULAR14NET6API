export interface ResponseDto<T> {
    data : T
    message: string;
    isSuccess: boolean;
}