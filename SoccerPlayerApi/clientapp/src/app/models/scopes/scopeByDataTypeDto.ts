import { GetScopeDataDto } from "./getScopeDataDto";

export type scopeByDataTypeDto = {
    [dataTypeId: number]: GetScopeDataDto[];
};